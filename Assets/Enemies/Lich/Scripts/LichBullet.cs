using System;
using PlayerScripts;
using UnityEngine;

namespace Enemies.Lich.Scripts
{
    public class LichBullet : MonoBehaviour
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
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject, 0.05f);
            }
        }
    }
}
