using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")] //change header in unity for easier reading/identifying
    [SerializeField]
    private float startHealth;

    public float currentHealth { get; private set; } //others can read but only here can change
    private Animator anim;
    private bool isDead;

    [Header("iframes")] 
    [SerializeField] private float iframeDuration;
    [SerializeField] private int numOfFlash;
    private SpriteRenderer spriteRenderer;

    [Header("Components")] [SerializeField]
    private Behaviour[] components;
    private bool invulnerable;
    
    [Header("Death Sound")]
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip hurt;
    private void Awake()
    {
        currentHealth = startHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, startHealth); //mathf.clamp for limit

        if (currentHealth > 0f)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerble());
            SoundManager.instance.PlaySound(hurt);
        }
        else
        {
            if (!isDead)
            {
                // deactivated all attached components
                foreach (Behaviour component in components)
                {
                    component.enabled = false;
                }
                anim.SetBool("grounded", true);
                anim.SetTrigger("die");
                isDead = true;
                SoundManager.instance.PlaySound(death);
                StartCoroutine(HideSpriteAfterDelay(1));
            }
        }
    }
    
    IEnumerator HideSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0f, startHealth);
    }

    private IEnumerator Invunerble()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(8, 9, true); //ignore collision w/ layers in arg
        for (int i = 0; i < numOfFlash; i++)
        {
            spriteRenderer.color = new Color(1,0,0, 0.5f);
            yield return new WaitForSeconds(iframeDuration / (numOfFlash * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iframeDuration / (numOfFlash * 2));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
        invulnerable = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        isDead = false;
        GetComponent<SpriteRenderer>().enabled = true;
        AddHealth(startHealth);
        anim.ResetTrigger("die");
        anim.Play("idle");
        StartCoroutine(Invunerble());
        
        // deactivated all attached components
        foreach (Behaviour component in components)
        {
            component.enabled = true;
        }
    }
}
