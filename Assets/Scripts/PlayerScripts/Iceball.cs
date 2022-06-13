using System.Collections;
using System.Collections.Generic;
using EnemyScripts;
using UnityEngine;
using UnityEngine.AI;

public class Iceball : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector3 shootDir;
    private Animator animator;
    private NavMeshAgent agent;
    public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir;
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject;
        if (enemy != null)
        {
            animator = enemy.GetComponent<Animator>();
            agent = enemy.GetComponent<NavMeshAgent>();
            StartCoroutine(Freeze());
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    private IEnumerator Freeze()
    {
        animator.speed = 0;
        agent.enabled = false;
        yield return new WaitForSeconds(1f);
        animator.speed = 1;
        agent.enabled = true;
        Destroy(gameObject);
    }
}
