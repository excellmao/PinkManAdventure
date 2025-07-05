using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack Params")]
    [SerializeField] private float attCd;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    
    [Header("Colliders Params")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float colliderDistance;
    
    [Header("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] projectile;
    
    [Header("Player Layers")]
    [SerializeField] private LayerMask player;
    private float cdTimer = Mathf.Infinity;
    
    private Animator anim;
    private EnemyPatrol enemyPatrol;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }
    

    private void Update()
    {
        cdTimer += Time.deltaTime;
        if (PlayerInSight())
        {
            if (cdTimer >= attCd)
            {
                cdTimer = 0;
                anim.SetTrigger("rangedattack");
            }
        }
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight(); //patrol if no player; stop patrol = hit if player present
    }

    private void RangedAttack()
    {
        cdTimer = 0;
        projectile[FindProjectile()].transform.position = firePoint.position;
        projectile[FindProjectile()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindProjectile()
    {
        for (int i = 0; i < projectile.Length; i++)
        {
            if (!projectile[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    
    //check if player in view?
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x  * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0f, Vector2.left, 0f,
            player);
        
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x  * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
