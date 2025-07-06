using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
   [SerializeField] private AudioClip checkpointSFX;
   private Transform currentCheckpoint;
   private Health playerHealth;
   private UIManager uiManager;

   private void Awake()
   {
      playerHealth = GetComponent<Health>();
      uiManager = FindObjectOfType<UIManager>(); //check all hier -> return 1st UIManager
   }

   public void CheckRespawn()
   {
      //check if checkpoint avail
      if (currentCheckpoint == null)
      {
         //gameover
         uiManager.GameOver();
         return;
      }
      
      transform.position = currentCheckpoint.position;
      playerHealth.Respawn();
      
      //move cam to checkpoint
      Camera.main.GetComponent<CameraMovement>().MovetoNew(currentCheckpoint.parent);
   }
   
   //activate checkpoint
   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.transform.CompareTag("Checkpoint"))
      {
         currentCheckpoint = collision.transform; //save checkpoint used
         SoundManager.instance.PlaySound(checkpointSFX);
         collision.GetComponent<Collider2D>().enabled = false; //deactivate checkpoint collider
         collision.GetComponent<Animator>().SetTrigger("appear");
      }
   }
}
