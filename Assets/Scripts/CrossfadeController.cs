using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossfadeController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void FadeToBlack()
    {
        animator.SetTrigger("Start");
    }
    
    
    public void FadeOutBlack()
    {
        animator.SetTrigger("End");
    }
}
