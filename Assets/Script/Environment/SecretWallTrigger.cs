using UnityEngine;

public class SecretWallTrigger : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private bool hasOpened = false;
    private bool isMoving = false;

    [SerializeField] private float moveDistance = 1.5f;
    [SerializeField] private float moveSpeed = 1f;

    private Transform wall; // 墙体本体（父对象）
    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        wall = transform.parent; // 假设触发器是墙的子物体
        closedPosition = wall.position;

        // 朝“后方”移动。这里用 -forward（即物体的后方方向）
        openPosition = closedPosition - wall.forward * moveDistance;
    }

    void Update()
    {
        if (isPlayerInRange && !hasOpened && Input.GetKeyDown(KeyCode.F))
        {
            hasOpened = true;
            isMoving = true;
        }

        if (isMoving)
        {
            wall.position = Vector3.MoveTowards(wall.position, openPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(wall.position, openPosition) < 0.01f)
            {
                wall.position = openPosition;
                isMoving = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
