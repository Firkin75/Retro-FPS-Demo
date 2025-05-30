using UnityEngine;

public class EnemySpriteController : MonoBehaviour
{
    private Transform player;
    private EnemyAI enemyAI;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAI = GetComponent<EnemyAI>();

    }

    void Update()
    {
        Vector3 moveDirection = enemyAI.MoveDirection;

        
       

        UpdateAnimation(moveDirection);
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.LookAt(player); // 保持 Billboard 效果
        }
    }

    void UpdateAnimation(Vector3 moveDirection)
    {
        float enemyAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float playerAngle = Mathf.Atan2(toPlayer.x, toPlayer.z) * Mathf.Rad2Deg;
        float angleDifference = (enemyAngle - playerAngle + 360) % 360;

        int directionIndex;
        bool flipX;
        GetDirectionAndFlip(angleDifference, out directionIndex, out flipX);

        animator.SetFloat("directionIndex", directionIndex);
        spriteRenderer.flipX = flipX;
    }

    void GetDirectionAndFlip(float angleDifference, out int directionIndex, out bool flipX)
    {
        flipX = false;
        if (angleDifference >= 337.5f || angleDifference < 22.5f)
            directionIndex = 0; // 正面
        else if (angleDifference >= 22.5f && angleDifference < 67.5f)
        {
            directionIndex = 1; flipX = true;
        }
        else if (angleDifference >= 67.5f && angleDifference < 112.5f)
        {
            flipX = true; directionIndex = 2;
        }
        else if (angleDifference >= 112.5f && angleDifference < 157.5f)
        {
            flipX = true; directionIndex = 3;
        }
        else if (angleDifference >= 157.5f && angleDifference < 202.5f)
            directionIndex = 4;
        else if (angleDifference >= 202.5f && angleDifference < 247.5f)
            directionIndex = 3;
        else if (angleDifference >= 247.5f && angleDifference < 292.5f)
            directionIndex = 2;
        else if (angleDifference >= 292.5f && angleDifference < 337.5f)
            directionIndex = 1;
        else
            directionIndex = 0;
    }
}
