using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector3 shootDir;
    private int damage;
    public void Setup(Vector3 shootDir, int damage)
    {
        this.shootDir = shootDir;
        this.damage = damage;
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Entity>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject, 0.05f);
        }
    }
}
