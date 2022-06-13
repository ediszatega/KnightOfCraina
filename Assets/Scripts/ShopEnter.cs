using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEnter : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject levelLoader;
    [SerializeField] private GameObject doorObject;

    private Transform door;
    private BoxCollider collider;
    private bool triggered;

    private void Awake()
    {
        door = doorObject.transform;
        collider = GetComponent<BoxCollider>();
        CloseDoor();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(MovePlayer());
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

    public IEnumerator MovePlayer()
    {
        if (!triggered)
        {
            triggered = true;
            levelLoader.GetComponent<CrossfadeController>().FadeToBlack();
            yield return new WaitForSecondsRealtime(2);
            camera.transform.position = new Vector3(-38.5f, 6, -10);
            player.transform.position = new Vector3(-40.71f, 0.582f, -1.74f);
            player.GetComponent<Rigidbody>().isKinematic = false;
            camera.GetComponent<AudioController>().PlayShopMusic();
            levelLoader.GetComponent<CrossfadeController>().FadeOutBlack();
            triggered = false;
        }
    }
}
