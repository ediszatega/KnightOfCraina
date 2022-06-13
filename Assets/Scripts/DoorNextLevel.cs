using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNextLevel : MonoBehaviour
{
   [SerializeField] private GameObject player;
   [SerializeField] private GameObject gameManagerObject;
   [SerializeField] private GameObject levelLoader;
   [SerializeField] private GameObject doorObject;

   private Transform door;
   private BoxCollider collider;
   private GameManager gameManager;
   private bool triggered;
   private void Awake()
   {
      door = doorObject.transform;
      collider = GetComponent<BoxCollider>();
      gameManager = gameManagerObject.GetComponent<GameManager>();
      CloseDoor();
   }

   public void CloseDoor()
   {
      triggered = false;
      door.GetComponent<MeshRenderer>().enabled = true;  
      collider.enabled = false;
   }
   
   public void OpenDoor()
   {
      door.GetComponent<MeshRenderer>().enabled = false;
      collider.enabled = true;
   }

   private void OnTriggerEnter(Collider other)
   {
      player.GetComponent<Rigidbody>().isKinematic = true;
      StartCoroutine(MovePlayer());
   }

   public IEnumerator MovePlayer()
   {
      if (!triggered)
      {
         triggered = true;
         levelLoader.GetComponent<CrossfadeController>().FadeToBlack();
         yield return new WaitForSeconds(2);
         player.transform.position = new Vector3(-13.24f, 0.582f, -2);
         player.GetComponent<Rigidbody>().isKinematic = false;
         gameManager.NextLevel();
         levelLoader.GetComponent<CrossfadeController>().FadeOutBlack();
         triggered = false;
      }
   }
}
