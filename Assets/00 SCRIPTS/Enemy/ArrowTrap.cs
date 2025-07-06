using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attCd;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;
    private float cdTimer;

    [Header("SFX")]
    [SerializeField] private AudioClip shoot;
    private void Attack()
    {
        cdTimer = 0;
        
        SoundManager.instance.PlaySound(shoot);
        arrows[FindArrows()].transform.position = firePoint.position;
        arrows[FindArrows()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindArrows()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    private void Update()
    {
        cdTimer += Time.deltaTime;
        if (cdTimer >= attCd)
        {
            Attack();
        }
    }
}
