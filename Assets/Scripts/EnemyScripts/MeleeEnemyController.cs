using System;
using System.Runtime.CompilerServices;
using PlayerScripts;
using ScriptableObjects;
using UnityEditor.Animations;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyScripts
 {
     [RequireComponent(typeof(EnemyMotor))]
     public class MeleeEnemyController : Entity
     {
         [SerializeField] private EntityData data;
    
         public  int maxHealth;
         public  int attackDamage;
         public  float attackRange;
         public  float attackCooldown; 
         public  float delay;
         public  float speed;
         public  float rotationSpeed;
         public  LayerMask enemyLayers;
    
         //Combat
         public int currentHealth;
         public double lastAttackedAt;
         public Transform attackPoint;


         //Movement
         private GameObject player;
         private EnemyMotor motor;

         public Animator animator;
         private NavMeshAgent agent;
         void Start()
         {
             ReadData();
             agent = GetComponent<NavMeshAgent>();
             agent.speed = speed;
             player = GameObject.FindGameObjectWithTag("Player");
             attackPoint = transform.Find("AttackPoint");
             motor = GetComponent<EnemyMotor>();
             animator = GetComponent<Animator>();
             currentHealth = maxHealth;
         }
         
         private void ReadData()
         {
             maxHealth = (int)(data.maxHealth + GameManager.level * 1.5);
             attackDamage = data.attackDamage;
             attackRange = data.attackRange;
             attackCooldown = data.attackCooldown - (int)(GameManager.level * 0.15f);
             delay = data.delay;
             speed = data.speed;
             rotationSpeed = data.rotationSpeed;
             enemyLayers = data.enemyLayers;
         }

         public bool CheckHit()
         {
             return Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers).Length > 0;
         }

         public bool CheckAttack()
         {
             return Time.time > lastAttackedAt + attackCooldown;
         }

         public override void Attack()
         {
             Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

             if (hitPlayer?.Length > 0)
             {
                 Debug.Log(hitPlayer.Length);
                 hitPlayer[0].GetComponent<PlayerController>().TakeDamage(attackDamage);
             }

             lastAttackedAt = Time.time;
         }

         public void MoveToPlayer()
         {
             motor.MoveToPoint(player.transform.position);
             var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);

             // Smoothly rotate towards the target point.
             transform.rotation =
                 Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
         }

         public void StopMoving()
         {
             motor.StopMoving();
         }

         public override int TakeDamage(int damage)
         {
             currentHealth -= damage;

             if (currentHealth <= 0)
             {
                 Die();
                 return maxHealth / 2;
             }
             animator.SetTrigger("Hurt");
             return 0;
         }

         public override void Die()
         {
             animator.SetTrigger("Die");
             this.enabled = false;
             motor.enabled = false;
             this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
             agent.enabled = false;
             Destroy(gameObject, 5f);
             GameManager.ReduceEnemies();
         }
     }
 }
