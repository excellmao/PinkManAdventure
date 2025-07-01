using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Animator _anim;
    private BoxCollider2D _boxCollider;
    private float wallJumpCd;
    private float horizontalInput;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //tell game 1 for right, -1 for left, value between if joystick, 0 when nothing; based on prj axis
        horizontalInput = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, _rb.velocity.y);
        
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
        
        //wallJump logic
        if (wallJumpCd > 0.1f)
        {
            _rb.velocity = new Vector2(horizontalInput * speed, _rb.velocity.y);
            if (onWall() && !isGrounded())
            {
                _rb.gravityScale = 0;
                _rb.velocity = Vector2.zero;
            } else _rb.gravityScale = 2.5f;
            
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }
        else wallJumpCd += Time.deltaTime;;
    }

    private void Jump()
    {
        if (isGrounded())
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            _anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            float pushDir = -Mathf.Sign(transform.localScale.x); //>< of wall
            _rb.velocity = new Vector2(pushDir * 10f, 7f);
            transform.localScale = new Vector3(pushDir * 2, transform.localScale.y, transform.localScale.z);
            // mathf.sign return sign of number; -1 if < 0, 1 if > 0
            wallJumpCd = 0;
        }
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
