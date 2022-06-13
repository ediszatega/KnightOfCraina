using System.Collections;
using EnemyScripts;
using ScriptableObjects;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : Entity
    {
        [SerializeField] private EntityData data;
        [SerializeField] private Transform fireball;
        [SerializeField] private Transform iceball;
        [SerializeField] private GameObject fireballShop;
        [SerializeField] private GameObject iceballShop;

        private int maxHealth;
        private int attackDamage;
        private float attackRange;
        private float attackCooldown; 
        private float delay;
        private float speed;
        private float rotationSpeed;
        private LayerMask enemyLayers;
    
        private CharacterController controller;
        private PlayerAudio audioController;
        private Animator animator;
    
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
    
        public int currentHealth;
        public int currentShield;
        public int currentCoins;

        private double lastAttackedAt;
        private Transform attackPoint;
        private bool isInvincible;
        public bool canMove;
        public int attackCounter;

        private bool hasFireball;
        private bool hasIceball;
        private bool dead;
        void Start ()
        {
            ReadData();
            currentHealth = maxHealth;
            currentShield = 0;
            currentCoins = 0;
            attackCounter = 0;
            hasFireball = false;
            hasIceball = false;
            isInvincible = false;
            dead = false;
            canMove = true;
            attackPoint = transform.Find("AttackPoint");
            controller = GetComponent<CharacterController>();
            audioController = GetComponent<PlayerAudio>();
            animator = GetComponent<Animator>();
        }

        private void ReadData()
        {
            maxHealth = data.maxHealth;
            attackDamage = data.attackDamage;
            attackRange = data.attackRange;
            attackCooldown = data.attackCooldown;
            delay = data.delay;
            speed = data.speed;
            rotationSpeed = data.rotationSpeed;
            enemyLayers = data.enemyLayers;
        }

        void Update ()
        {
            if(canMove)
                Move();
            if (CheckAttack())
            {
                StartAttack();
            }
        }

        public void MoveTo(Vector3 vector3)
        {
            this.gameObject.transform.position = vector3;
        }

        private void StartAttack()
        {
            StartCoroutine(AttackAnimation());
            lastAttackedAt = Time.time;
        }

        private bool CheckAttack()
        {
            return Input.GetKeyDown(KeyCode.Space) && Time.time > lastAttackedAt + attackCooldown && !dead;
        }

        public override void Attack()
        {
            attackCounter++;
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
            Debug.Log(hitEnemies.Length);
            if(hitEnemies.Length < 1)
                audioController.PlayMiss();
            else
                audioController.PlayHit();
            foreach (var enemy in hitEnemies)
            {
                currentCoins += enemy.GetComponent<Entity>().TakeDamage(attackDamage);
            }

            if (hasFireball)
            {
                SpawnSpell(fireball);
            }
            else if(hasIceball && attackCounter % 3 == 0)
            {
                SpawnSpell(iceball);
            }
        }

        private void SpawnSpell(Transform spell)
        {
            var spellPosition = attackPoint.position;
            Transform spellTransform = Instantiate(spell, spellPosition, Quaternion.identity);

            Vector3 shootDir = (transform.position + Vector3.up - spellPosition).normalized;
            if(spell == fireball)
                spellTransform.GetComponent<Fireball>().Setup(-shootDir, 5);
            else
                spellTransform.GetComponent<Iceball>().Setup(-shootDir);
        }
        
        public override int TakeDamage(int dmg)
        {
            if (!isInvincible)
            {
                audioController.PlayHurt();
                StartCoroutine(BecomeInvincible());

                if (currentShield != 0)
                {
                    GetComponent<DamageFlash>().FlashStart(Color.green);
                    currentShield -= dmg;
                    if (currentShield < 0)
                        currentShield = 0;
                }
                else
                {     
                    GetComponent<DamageFlash>().FlashStart(Color.red);
                    currentHealth -= dmg;
                    if (currentHealth < 0)
                        currentHealth = 0;
                }

                if (currentHealth == 0)
                {
                    Die();
                    return 0;
                }
                StartCoroutine(HitPause());
            }
            return 0;
        }

        private IEnumerator BecomeInvincible()
        {
            isInvincible = true;
            GetComponent<CharacterController>().radius = 0;
            yield return new WaitForSeconds(1f);
            isInvincible = false;
            GetComponent<CharacterController>().radius = 0.5f;
        }

        private IEnumerator HitPause()
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(0.35f);
            Time.timeScale = 1;
        }

        public override void Die()
        {
            Time.timeScale = 0;
            if(!dead)
                DeathPopUpUI.Instance.SetTitle("YOU DIED").Show();
            dead = true;
        }
        
        private void Move()
        {
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");

            if (gameObject.GetComponent<Rigidbody>().isKinematic || dead)
            {
                verticalInput = 0;
                horizontalInput = 0;
            }
            
            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
            movementDirection.Normalize();

            if(controller != null && controller.enabled)
                controller.SimpleMove(movementDirection * speed);
        
            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation,
                    rotationSpeed * Time.deltaTime);

                Run();
            }
            else
            {
                Idle();
            }
        }

        private void Idle()
        {
            animator.SetFloat(Speed, 0, 0.1f, Time.deltaTime);
        }

        private void Run()
        {
            animator.SetFloat(Speed, 1, 0.1f, Time.deltaTime);
        }

        private IEnumerator AttackAnimation()
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);
            animator.SetTrigger(Attack1);

            yield return new WaitForSeconds(0.9f);
            animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
        }

        public void Heal()
        {
            currentHealth = maxHealth;
        }

        public void SetUpgrade(int upgrade)
        {
            switch (upgrade)
            {
                case 1:
                    maxHealth += 1;
                    currentCoins -= 15;
                    Heal();
                    break;
                case 2:
                    currentShield += 2;
                    currentCoins -= 10;
                    break;
                case 3:
                    attackDamage += 5;
                    currentCoins -= 25;
                    break;
                case 4:
                    speed += 1;
                    currentCoins -= 10;
                    break;
                case 5:
                    hasFireball = true;
                    hasIceball = false;
                    fireballShop.GetComponent<MeshRenderer>().enabled = false;
                    fireballShop.GetComponent<ProductShopping>().enabled = false;
                    
                    iceballShop.GetComponent<MeshRenderer>().enabled = true;
                    iceballShop.GetComponent<ProductShopping>().enabled = true;
                    currentCoins -= 40;
                    break;
                case 6:
                    hasFireball = false;
                    hasIceball = true;
                    fireballShop.GetComponent<MeshRenderer>().enabled = true;
                    fireballShop.GetComponent<ProductShopping>().enabled = true;
                    
                    iceballShop.GetComponent<MeshRenderer>().enabled = false; 
                    iceballShop.GetComponent<ProductShopping>().enabled = false;
                    currentCoins -= 40;
                    break;
            }
        }
    }
}
