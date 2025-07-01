using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform prevRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraMovement cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.x < transform.position.x)
            {
                cam.MovetoNew(nextRoom);
            } else cam.MovetoNew(prevRoom); 
        }
    }
}
