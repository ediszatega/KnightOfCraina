using System.Collections;
using System.Collections.Generic;
using Enemies.Lich.Scripts;
using PlayerScripts;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyController : Entity
{
    [SerializeField] private EntityData data;
    [SerializeField] private Transform bullet;

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
    public bool canAttack;
    
    private GameObject player;
    public Animator animator;
    void Start()
    {
        ReadData();
        player = GameObject.FindGameObjectWithTag("Player");
        attackPoint = transform.Find("AttackPoint");
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        canAttack = true;
    }
         
    private void ReadData()
    {
        maxHealth = (int)(data.maxHealth + GameManager.level * 1.25f);
        attackDamage = data.attackDamage;
        attackRange = data.attackRange;
        attackCooldown = data.attackCooldown;
        delay = data.delay;
        speed = data.speed;
        rotationSpeed = data.rotationSpeed;
        enemyLayers = data.enemyLayers;
    }

    public void RotateToPlayer()
    {
        var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public override void Attack()
    {
        if (canAttack)
        {
            var bulletPosition = transform.position + Vector3.up;
            Transform bulletTransform = Instantiate(bullet, bulletPosition, Quaternion.identity);

            Vector3 shootDir = (player.transform.position + Vector3.up - bulletPosition).normalized;
            bulletTransform.GetComponent<LichBullet>().Setup(shootDir, attackDamage);

            lastAttackedAt = Time.time;
        }
    }
    
    public bool CheckAttack()
    {
        return Time.time > lastAttackedAt + attackCooldown && canAttack;
    }


    public override int TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Die();
            return maxHealth/2;
        }
        animator.SetTrigger("Hurt");
        return 0;
    }

    public override void Die()
    {
        animator.SetTrigger("Die");
        this.enabled = false;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(gameObject, 5f);
        GameManager.ReduceEnemies();
    }
}
