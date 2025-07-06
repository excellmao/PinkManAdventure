using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;
    
    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Walljumping")] 
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;
    
    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    
    private Animator _anim;
    private BoxCollider2D _boxCollider;
    private float wallJumpCd;
    private float horizontalInput;
    
    [Header("SFX")]
    [SerializeField] private AudioClip jumpSFX;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //tell game 1 for right, -1 for left, value between if joystick, 0 when nothing; based on prj axis
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);
        
        //player flip
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(2, 2, 1);
        } 
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }
        
        //animate
        _anim.SetBool("run", horizontalInput != 0);
        _anim.SetBool("grounded", isGrounded());
        _anim.SetBool("onWall", onWall() && !isGrounded());
        
        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        //jump height change
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2f);
        }

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 2.5f;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //reset coyote
                jumpCounter = extraJumps;
            }
            else
            {
                coyoteCounter -= Time.deltaTime; //touch ground -> decrease coyote
            }
        }
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;
        SoundManager.instance.PlaySound(jumpSFX);

        if (onWall())
            wallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpForce);
            else
            {
                if (coyoteCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jumpForce);
                else
                {
                    if (jumpCounter > 0)
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpForce);
                        jumpCounter--;
                    }
                }
            }
            
            coyoteCounter = 0;
        }
    }

    private void wallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
    }
    
    private bool isGrounded()
    {   
        //boxcast = use box to cast check ground =/= only 1 ray\
        //layer mask = cast only look for the layer; ignore other
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center,_boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    
    //use same for wall
    private bool onWall()
    {
        //transform localScale x for checking x-axis for wall
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center,_boxCollider.bounds.size, 0f, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
