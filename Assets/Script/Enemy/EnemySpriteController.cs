using UnityEngine;

public class EnemySpriteController : MonoBehaviour
{
    private Transform player;            // Reference to the player
    private EnemyAI enemyAI;             // Reference to the enemy's AI script
    private Animator animator;           // Animator for sprite animation
    private SpriteRenderer spriteRenderer; // SpriteRenderer to flip sprites

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

        // Update sprite animation direction
        UpdateAnimation(moveDirection);
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.LookAt(player); // Maintain billboard effect (face the camera/player)
        }
    }

    // Determine animation direction and flip based on movement and player position
    void UpdateAnimation(Vector3 moveDirection)
    {
        // Direction the enemy is moving toward
        float enemyAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

        // Direction from enemy to player
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float playerAngle = Mathf.Atan2(toPlayer.x, toPlayer.z) * Mathf.Rad2Deg;

        // Difference in angle between movement direction and player direction
        float angleDifference = (enemyAngle - playerAngle + 360) % 360;

        int directionIndex;
        bool flipX;
        GetDirectionAndFlip(angleDifference, out directionIndex, out flipX);

        // Set animation parameters
        animator.SetFloat("directionIndex", directionIndex);
        spriteRenderer.flipX = flipX;
    }

    // Determine which direction index to use and whether to flip the sprite horizontally
    void GetDirectionAndFlip(float angleDifference, out int directionIndex, out bool flipX)
    {
        flipX = false;

        if (angleDifference >= 337.5f || angleDifference < 22.5f)
            directionIndex = 0; // Front
        else if (angleDifference >= 22.5f && angleDifference < 67.5f)
        {
            directionIndex = 1;
            flipX = true;
        }
        else if (angleDifference >= 67.5f && angleDifference < 112.5f)
        {
            directionIndex = 2;
            flipX = true;
        }
        else if (angleDifference >= 112.5f && angleDifference < 157.5f)
        {
            directionIndex = 3;
            flipX = true;
        }
        else if (angleDifference >= 157.5f && angleDifference < 202.5f)
            directionIndex = 4; // Back
        else if (angleDifference >= 202.5f && angleDifference < 247.5f)
            directionIndex = 3;
        else if (angleDifference >= 247.5f && angleDifference < 292.5f)
            directionIndex = 2;
        else if (angleDifference >= 292.5f && angleDifference < 337.5f)
            directionIndex = 1;
        else
            directionIndex = 0; // Fallback
    }
}
