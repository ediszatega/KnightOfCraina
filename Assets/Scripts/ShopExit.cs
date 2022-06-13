using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using UnityEngine.AI;

public class ShopExit : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject levelLoader;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(MovePlayer());
    }

    public IEnumerator MovePlayer()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            levelLoader.GetComponent<CrossfadeController>().FadeToBlack();
            yield return new WaitForSecondsRealtime(2);
            camera.transform.position = new Vector3(-13.5f, 6, -10);
            player.transform.position = new Vector3(-9.78f, 0.582f, 0.48f);
            player.GetComponent<Rigidbody>().isKinematic = false;
            camera.GetComponent<AudioController>().PlayCombatMusic();
            levelLoader.GetComponent<CrossfadeController>().FadeOutBlack();
            isTriggered = false;
        }
    }
}
