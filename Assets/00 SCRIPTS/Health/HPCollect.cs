using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCollect : MonoBehaviour
{
    [SerializeField] private float value;
    [SerializeField] private AudioClip pickupSFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(pickupSFX);
            collision.GetComponent<Health>().AddHealth(value);
            gameObject.SetActive(false);
        }
    }
}
