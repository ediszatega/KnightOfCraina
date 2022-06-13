using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private SkinnedMeshRenderer meshRenderer;
    private Color originColor;
    private float flashTime = 0.15f;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = meshRenderer.material.color;
    }

    public void FlashStart(Color c)
    {
        meshRenderer.material.color = c;
        Invoke("FlashStop", flashTime);
    }

    public void FlashStop()
    {
        meshRenderer.material.color = originColor;
    }
}
