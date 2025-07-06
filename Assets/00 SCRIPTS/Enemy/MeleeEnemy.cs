using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Params")]
    [SerializeField] private float attCd;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    
    [Header("Colliders Params")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float colliderDistance;
    
    [Header("Player Layers")]
    [SerializeField] private LayerMask player;
    private float cdTimer = Mathf.Infinity;
    
    [Header("Sound Params")]
    [SerializeField]  private AudioClip swordSlash;
    
    private Animator anim;
    private Health playerHealth;

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
            if (cdTimer >= attCd && playerHealth.currentHealth > 0)
            {
                cdTimer = 0;
                anim.SetTrigger("meleeattack");
                SoundManager.instance.PlaySound(swordSlash);
            }
        }
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight(); //patrol if no player; stop patrol = hit if player present
    }

    //check if player in view?
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x  * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0f, Vector2.left, 0f,
            player);
        if (hit.collider != null)
            playerHealth = hit.collider.GetComponent<Health>();
        
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x  * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
