using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Go());
    }

    IEnumerator Go()
    {
        while (true)
        {
            this.gameObject.GetComponent<Animation>().Play();
            yield return new WaitForSeconds(3f);
        }
    }
}
