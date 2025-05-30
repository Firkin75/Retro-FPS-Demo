using UnityEngine.AI;
using UnityEngine;



using System.Collections;

public class EnemyAI : MonoBehaviour
{



   
    public LayerMask Player;
    public float health;
    public GameObject defaultDropItem;
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

   

    public Transform firePoint; // 在 Unity Inspector 里设置 FirePoint
    public int damage;
    public Transform dropPoint;
    public AudioSource atkSound;

    private Transform player;
    private bool isAlive = true;
    private NavMeshAgent agent;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color flashColor = Color.red;  // 设定闪红色
    public float flashDuration;    // 闪烁持续时间
    public int flashCount = 1;            // 闪烁次数
    public bool isAggro;




    // 视野和攻击范围
    public float sightRange, attackRange;
    protected bool playerInSightRange, playerInAttackRange;
    




    // 公开的移动方向（供 EnemySpriteController 访问）
    public Vector3 MoveDirection { get; private set; }

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the Player GameObject is tagged as 'Player'.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;  // 获取原本颜色
        anim = GetComponent<Animator>();

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.updateRotation = false; // 禁用 NavMesh 自动旋转
        agent.updateUpAxis = false;   // 禁用 NavMesh 轴变换
    }

    void Update()
    {
        if (!isAlive) return;

        // 根据速度控制 isMoving
        anim.SetBool("isMoving", agent.velocity.sqrMagnitude > 0.01f);

        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        float distanceToPlayer = Vector3.Distance(firePoint.position, player.position);

        // 视野检测
        playerInSightRange = false;
        if (distanceToPlayer <= sightRange)
        {
            // 定义检测层：不包括 TriggerCollider，包含所有正常的可视层
            int visionMask = ~LayerMask.GetMask("TriggerCollider");

            // 允许Trigger参与检测，确保玩家被检测到
            if (Physics.Raycast(firePoint.position, directionToPlayer, out RaycastHit sightHit, sightRange, visionMask, QueryTriggerInteraction.Collide))
            {
                if (sightHit.collider.CompareTag("Player"))
                {
                    playerInSightRange = true;
                }
            }
        }

        // 攻击范围检测
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

        if ((isAggro || playerInSightRange) && !playerInAttackRange)
        {
            ChasePlayer();
           
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
        else if (!playerInSightRange && !playerInAttackRange && !isAggro)
        {
            Patrolling();
            
        }



        // 计算移动方向
        if (agent.isStopped)
        {
            MoveDirection = Vector3.zero;

        }
        else
            MoveDirection = agent.velocity.normalized;




    }


    protected virtual void Patrolling()
    {
        if (patrolPoints.Length == 0)
        {
            // 没有巡逻点，敌人静止
            agent.isStopped = true;
            return;
        }
        anim.SetBool("isAttacking", false);
        agent.isStopped = false;

        // 到达当前巡逻点后，切换下一个
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
        // 停止移动
        agent.SetDestination(transform.position);

        // 面向玩家
        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(directionToPlayer), Time.deltaTime * 5f);

        // 播放攻击动画
        anim.SetBool("isAttacking", true);

       


    }

    public void raycastAttack()
    {
        
        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;

        // 朝玩家转向（可选）
        transform.rotation = Quaternion.LookRotation(directionToPlayer);

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, directionToPlayer, out hit, attackRange))
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
        playerInSightRange = true;
        isAggro = true;
        agent.isStopped = false;
        if (health <= 0)
        {

            Die();
        }

    }
    public IEnumerator FlashRed()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;  // 改为红色
            yield return new WaitForSeconds(flashDuration);  // 等待一段时间
            spriteRenderer.color = originalColor;  // 恢复原颜色
            yield return new WaitForSeconds(flashDuration);  // 等待一段时间
        }
    }

    public virtual void Die()
    {
        if (!isAlive) return; // 防止重复调用
        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null)
        {
            box.enabled = false;
        }
        isAlive = false;
        agent.enabled = false;

        // 重置所有动画状态
        anim.SetBool("isAttacking", false);

        anim.SetBool("isMoving", false);

        // 确保Trigger被正确触发
        anim.ResetTrigger("Die"); // 先重置
        anim.SetTrigger("Die");   // 再触发

        /*CancelInvoke(nameof(ResetAttack));*/
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
            // 在敌人死亡位置生成武器
            GameObject dropItem = Instantiate(defaultDropItem, dropPosition, Quaternion.identity);

            // 给武器添加初始物理弹力（如果武器有 Rigidbody）
            Rigidbody rb = dropItem.GetComponent<Rigidbody>();

        }


    }

    public void OnHeardGunshot()
    {
        if (!isAlive) return;

        isAggro = true;
       
    }

    public virtual GameObject GetDropItem()
    {
        return defaultDropItem; // 默认掉落的物品（可以在子类中重写）
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    
}


