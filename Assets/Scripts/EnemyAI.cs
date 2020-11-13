using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace FSM
{
    public class EnemyAI : MonoBehaviour
    {
        public NavMeshAgent agent;
        public Transform player;
        public LayerMask whatIsGround, whatIsPlayer;
        public Rigidbody rigidbody;

        public float maxHealth = 150;
        public Vector3 walkPoint;
        bool walkPointSet;
        public float walkPointRange;
        public float health;
        public float timeBetweenAttacks;
        bool alreadyAttacked;

        public float sightRange, attackRange;
        public bool playerInSightRange, playerInAttackRange;

        public Animator anim;
        private string[] attacks = { "Attack", "Attack2", "Attack3" };
        int attack = 0;
        public GameObject enemy;
        public GameObject healthBarUI;
        public Slider slider;

        void Start()
        {
            slider.value = CalculateHealth();
            health = maxHealth;
        }

        // Start is called before the first frame update
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (enemy != null && health > 0)
            {
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

                if (!playerInSightRange && !playerInAttackRange) Patroling();
                if (playerInSightRange && !playerInAttackRange) ChasePlayer();
                if (playerInSightRange && playerInAttackRange) AttackPlayer();

                if (health < maxHealth)
                {
                    healthBarUI.SetActive(true);
                }

                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                slider.value = CalculateHealth();
            }
        }

        private void FixedUpdate()
        {
            if (enemy != null)
            {
                if (playerInAttackRange)
                {
                    rigidbody.isKinematic = true;
                }
            }
        }

        private void Patroling()
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position = walkPoint;
            //walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f || playerInAttackRange)
                walkPointSet = false;
        }

        private void SearchWalkPoint()
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            {
                walkPointSet = true;
            }
        }
        private void ChasePlayer()
        {
            agent.SetDestination(player.position);
        }

        private void AttackPlayer()
        {
            //Make sure enemy doesn't move
            agent.SetDestination(transform.position);

            transform.LookAt(player);

            if (!alreadyAttacked)
            {
                alreadyAttacked = true;
                anim.SetTrigger(attacks[attack]);
                agent.isStopped = true;
                if (health > 0)
                {
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
                if (attack == 3)
                {
                    attack = 0;
                }
            }
            if (alreadyAttacked)
            {
                anim.SetTrigger("Dodge");
            }
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
            agent.isStopped = false;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            CalculateHealth();
            anim.SetTrigger("Hit");
            do
            {

            } while (anim.GetCurrentAnimatorStateInfo(0).IsTag("Hit"));

            if (health <= 0)
            {
                healthBarUI.SetActive(false);
                anim.SetTrigger("Die");
                agent.isStopped = true;
                player.GetComponentInParent<PlayerStateManager>().OnClearLookOverride();
                agent.GetComponentInParent<EnemyAI>().enabled = false;
                Invoke(nameof(DestroyEnemy), 1);
            }
        }

        private float CalculateHealth()
        {
            return health / maxHealth;
        }

        private void DestroyEnemy()
        {
            Destroy(enemy);
        }
    }
}