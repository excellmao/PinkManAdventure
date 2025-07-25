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
                nextRoom.GetComponent<Room>().Activate(true);
                prevRoom.GetComponent<Room>().Activate(false);
            }
            else
            {
                cam.MovetoNew(prevRoom);
                prevRoom.GetComponent<Room>().Activate(true);
                nextRoom.GetComponent<Room>().Activate(false);
            } 
        }
    }
}
