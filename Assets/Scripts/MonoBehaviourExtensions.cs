using System;
using System.Collections;
using UnityEngine;

static class MonoBehaviourExtensions
{
    public static IEnumerator Delay(this MonoBehaviour mb, float period, Action predicate)
    {
        yield return new WaitForSeconds(period);
        predicate.Invoke();
    }
}