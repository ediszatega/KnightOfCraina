using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip combatMusic;
    [SerializeField] private AudioClip shopMusic;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCombatMusic()
    {
        audioSource.clip = combatMusic;
        audioSource.Play();
    }

    public void PlayShopMusic()
    {
        audioSource.clip = shopMusic;
        audioSource.Play();
    }
}
