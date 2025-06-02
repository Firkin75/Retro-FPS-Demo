using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Vector3 MoveDirection { get; private set; } // Public move direction for sprite controller
    public bool isDeaf;                                // If true, enemy won't respond to sound

    public LayerMask Player;
    public float health;
    public GameObject defaultDropItem;                 // Item to drop on death
    public Transform[] patrolPoints;                   // Patrol route
    private int currentPatrolIndex = 0;

    public Transform firePoint;                        // Raycast origin for attacks
    public int damage;
    public Transform dropPoint;                        // Drop location for items
    public AudioSource atkSound;

    private Transform player;
    private bool isAlive = true;
    private NavMeshAgent agent;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color flashColor = Color.red;               // Damage flash color
    public float flashDuration;
    public int flashCount = 1;
    public bool isAggro;                               // If enemy has detected the player

    // Detection and attack range
    public float sightRange, attackRange;
    protected bool playerInSightRange, playerInAttackRange;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player not found!");

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;  // Prevent auto-rotation (billboard/sprite-friendly)
        agent.updateUpAxis = false;    // Keep enemy upright on 2D/flat plane

        // Improve pathing and prevent enemies from clumping
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.avoidancePriority = Random.Range(30, 70);
    }

    void Update()
    {
        if (!isAlive) return;

        // Animation based on movement
        anim.SetBool("isMoving", agent.velocity.sqrMagnitude > 0.01f);

        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        float distanceToPlayer = Vector3.Distance(firePoint.position, player.position);

        // Sight check using raycast
        playerInSightRange = false;
        if (distanceToPlayer <= sightRange)
        {
            int visionMask = LayerMask.GetMask("Player", "Environment");
            if (Physics.Raycast(firePoint.position, directionToPlayer, out RaycastHit hit, sightRange, visionMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    playerInSightRange = true;
                    isAggro = true;
                }
            }
        }

        // Attack range check
        playerInAttackRange = false;
        if (distanceToPlayer <= attackRange)
        {
            if (Physics.Raycast(firePoint.position, directionToPlayer, out RaycastHit attackHit, attackRange))
            {
                if (attackHit.collider.CompareTag("Player"))
                {
                    playerInAttackRange = true;
                }
            }
        }

        // AI behavior state machine
        if (isAggro && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
        else if (!isAggro)
        {
            Patrolling();
        }

        // Update move direction for animation
        MoveDirection = agent.isStopped ? Vector3.zero : agent.velocity.normalized;
    }

    protected virtual void Patrolling()
    {
        if (patrolPoints.Length == 0)
        {
            agent.isStopped = true;
            return;
        }

        anim.SetBool("isAttacking", false);
        agent.isStopped = false;

        // Move to next patrol point
        if (!agent.pathPending && agent.remainingDistance <= 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    protected virtual void ChasePlayer()
    {
        if (!isAlive) return;
        agent.isStopped = false;
        agent.SetDestination(player.position);
        anim.SetBool("isAttacking", false);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position); // Stop moving

        // Face the player
        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * 5f);

        anim.SetBool("isAttacking", true);
    }

    // Called by animation event
    public void raycastAttack()
    {
        Debug.Log("Attack triggered!");

        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        transform.rotation = Quaternion.LookRotation(directionToPlayer);

        if (Physics.Raycast(firePoint.position, directionToPlayer, out RaycastHit hit, attackRange))
        {
            atkSound?.Play();

            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (!isAlive) return;

        StartCoroutine(FlashRed());
        health -= damage;

        // Become aggressive when damaged
        playerInSightRange = true;
        isAggro = true;
        agent.isStopped = false;

        if (health <= 0)
        {
            Die();
        }
    }

    // Flash red effect when hit
    public IEnumerator FlashRed()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    public virtual void Die()
    {
        if (!isAlive) return;

        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null) box.enabled = false;

        isAlive = false;
        agent.enabled = false;

        anim.SetBool("isAttacking", false);
        anim.SetBool("isMoving", false);

        anim.ResetTrigger("Die");
        anim.SetTrigger("Die");

        DropItem();
    }

    public virtual void DestroyGameObj()
    {
        Destroy(gameObject);
    }

    private void DropItem()
    {
        if (defaultDropItem != null)
        {
            Vector3 dropPosition = dropPoint.position;
            GameObject dropItem = Instantiate(defaultDropItem, dropPosition, Quaternion.identity);

            // Optional: Add physics if the item has Rigidbody
            Rigidbody rb = dropItem.GetComponent<Rigidbody>();
        }
    }

    // Called when player fires a gun nearby
    public void OnHeardGunshot()
    {
        if (!isAlive) return;
        if (!isDeaf)
        {
            isAggro = true;
        }
    }

    public virtual GameObject GetDropItem()
    {
        return defaultDropItem;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
