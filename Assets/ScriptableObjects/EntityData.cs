using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEntityData", menuName = "Entity")]
    public class EntityData : ScriptableObject
    {
        public int maxHealth;
        public int attackDamage;
        public float attackRange;
        public float attackCooldown;
        public float delay;
        public float speed;
        public float rotationSpeed;
        public LayerMask enemyLayers;
    }
}
