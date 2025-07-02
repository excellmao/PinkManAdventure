using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attCd;
    [SerializeField] private Transform bulletStart;
    [SerializeField] private GameObject[] bullets;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cdTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cdTimer > attCd && playerMovement.canAttack())
        {
            Attack();
        }
        cdTimer += Time.deltaTime;
    }

    private void Attack()
    {
        cdTimer = 0;
        bullets[FindBullets()].transform.position = bulletStart.position;
        bullets[FindBullets()].GetComponent<Bullet>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindBullets()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
        
}
