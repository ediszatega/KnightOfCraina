using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip missSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip hurtSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMiss()
    {
        audioSource.clip = missSound;
        audioSource.Play();
    }
    
    public void PlayHit()
    {
        audioSource.clip = hitSound;
        audioSource.Play();
    }
    
    public void PlayHurt()
    {
        audioSource.clip = hurtSound;
        audioSource.Play();
    }
}
