using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;
    
    [Header("Firetrap Timers")] 
    [SerializeField] private float activateDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    
    private bool triggered;
    private bool active;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!triggered)
            {
                StartCoroutine(ActivateFireTrap());
            }

            if (active)
            {
                collision.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }

    private IEnumerator ActivateFireTrap()
    {
        //trigger -> turn red to tell user
        triggered = true;
        anim.SetBool("hit", true);
        
        //wait then turn on
        yield return new WaitForSeconds(activateDelay);
        spriteRenderer.color = Color.white;
        active = true;
        anim.SetBool("activated", true);
        
        //wait -> deactivate -> reset
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("hit", false);
        anim.SetBool("activated", false);
    }
}
