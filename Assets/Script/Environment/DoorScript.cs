using System.IO.IsolatedStorage;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject enemySet;     // Optional: enemy group to activate when door opens
    public bool enemyOpenable;      // Whether enemies are allowed to open this door
    public bool isLocked;           // (Unused) Lock state for future extension

    private Transform doorTransform;               // Reference to the door mesh/visual
    private Vector3 openOffset = new Vector3(3, 0, 0); // Offset to move when opening
    [SerializeField] private float moveSpeed = 2f;      // Speed of door movement
    [SerializeField] private float autoCloseDelay = 5f; // Time to wait before auto-close

    private Vector3 closedPosition; // Original (closed) position
    private Vector3 openPosition;   // Target (open) position
    private bool isPlayerInRange = false;
    private bool isEnemyInRange = false;
    private bool isOpen = false;
    private float lastOpenTime = -999f; // Timestamp of last open

    void Start()
    {
        // Assume the door object is a child named "Door"
        Transform parent = transform.parent;
        if (parent != null)
        {
            doorTransform = parent.Find("Door");
        }

        if (doorTransform == null)
        {
            return; // Exit if no door found
        }

        closedPosition = doorTransform.position;
        openPosition = closedPosition + doorTransform.up * openOffset.magnitude; // Move along door's local up direction
    }

    void Update()
    {
        if (doorTransform == null) return;

        // Player manually opens the door by pressing F
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            OpenDoor();

            // Optionally activate enemy group when door is opened
            if (enemySet != null) enemySet.SetActive(true);
        }

        // Auto-close after delay if neither player nor enemy is nearby
        if (isOpen && !isPlayerInRange && !isEnemyInRange && Time.time - lastOpenTime >= autoCloseDelay)
        {
            CloseDoor();
        }

        // Smooth movement between open/closed positions
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;
        doorTransform.position = Vector3.MoveTowards(doorTransform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void OpenDoor()
    {
        isOpen = true;
        lastOpenTime = Time.time;
    }

    private void CloseDoor()
    {
        isOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
        else if (other.CompareTag("Enemy"))
        {
            if (!enemyOpenable)
            {
                return;
            }
            else
            {
                isEnemyInRange = true;
                OpenDoor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
        else if (other.CompareTag("Enemy"))
        {
            isEnemyInRange = false;
        }
    }
}
