using UnityEngine;

public class DoorScript : MonoBehaviour
{
   
    public GameObject enemySet;
    public bool enemyOpenable;

    private Transform doorTransform;
    private Vector3 openOffset = new Vector3(3, 0, 0);
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float autoCloseDelay = 5f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isPlayerInRange = false;
    private bool isEnemyInRange = false;
    private bool isOpen = false;
    private float lastOpenTime = -999f;

    void Start()
    {
        Transform parent = transform.parent;
        if (parent != null)
        {
            doorTransform = parent.Find("Door");
        }

        if (doorTransform == null)
        {
           
            return;
        }

        closedPosition = doorTransform.position;
        openPosition = closedPosition + doorTransform.up * openOffset.magnitude;

    }

    void Update()
    {
        if (doorTransform == null) return;

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            OpenDoor();
            if (enemySet != null) enemySet.SetActive(true);
        }

        if (isOpen && !isPlayerInRange && !isEnemyInRange && Time.time - lastOpenTime >= autoCloseDelay)
        {
            CloseDoor();
        }

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
            if (enemyOpenable == false)
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
