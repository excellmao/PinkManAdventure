using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeHead : EnemyDamage
{
    [Header("Spike Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask player;
    private Vector3[] directions =  new Vector3[4];
    private float checkTimer;
    private Vector3 dest;
    private bool attacking;
    

    private void OnEnable()
    {
        Stop();
    }

    private void Update()
    {
        
        if (attacking)
            transform.Translate(dest * Time.deltaTime * speed);
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
            {
                CheckForPlayer();
            }
        }
    }
 
    private void CheckForPlayer()
    {
        CalcDirection();
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, player);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                dest = directions[i];
                checkTimer = 0;
            }
        }  
    }
    
    //check if spikehead sees player from all directions
    private void CalcDirection()
    {
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range; //left
        directions[2] = transform.up * range;
        directions[3] = -transform.up * range; //down
    }

    private void Stop()
    {
        dest = transform.position; //set dest as current -> doesnt move
        attacking = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        //stop spikehead when hit
        Stop();
    }
}
