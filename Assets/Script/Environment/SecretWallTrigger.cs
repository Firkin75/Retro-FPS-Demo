using UnityEngine;

public class SecretWallTrigger : MonoBehaviour
{
    private bool isPlayerInRange = false; // Whether the player is inside the trigger
    private bool hasOpened = false;       // Whether the wall has already opened
    private bool isMoving = false;        // Whether the wall is currently moving

    [SerializeField] private float moveDistance = 1.5f; // Distance to move the wall
    [SerializeField] private float moveSpeed = 1f;      // Speed of wall movement

    private Transform wall;               // The wall object (assumed to be the parent)
    private Vector3 closedPosition;       // Original position of the wall
    private Vector3 openPosition;         // Target position when wall is open

    void Start()
    {
        wall = transform.parent; // Assumes the trigger is a child of the wall
        closedPosition = wall.position;

        // Calculate the open position by moving the wall backward relative to its own orientation
        openPosition = closedPosition + wall.TransformDirection(Vector3.back * moveDistance);
    }

    void Update()
    {
        // When player is in range and presses F, begin opening
        if (isPlayerInRange && !hasOpened && Input.GetKeyDown(KeyCode.F))
        {
            hasOpened = true;
            isMoving = true;
        }

        // Move the wall towards the open position
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

    // Detect when the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
