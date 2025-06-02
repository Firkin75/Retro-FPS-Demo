using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public GameObject enemySet;
    public bool reqKey1;
    public bool reqKey2;

    private Transform doorTransform;
    private Vector3 openOffset = new Vector3(3, 0, 0);
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isPlayerInRange = false;
    private bool hasKey1;
    private bool hasKey2;
    private bool isOpen = false;
   
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

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && reqKey1 && hasKey1)
        {
            OpenDoor();
            if (enemySet != null) enemySet.SetActive(true);
        }

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && reqKey2 && hasKey2)
        {
            OpenDoor();
            if (enemySet != null) enemySet.SetActive(true);
        }


        Vector3 targetPosition = isOpen ? openPosition : closedPosition;
        doorTransform.position = Vector3.MoveTowards(doorTransform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void OpenDoor()
    {
        isOpen = true;
        
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerInventory>().hasKey1)
        {
            isPlayerInRange = true;
            hasKey1 = true;
        }

        if (other.CompareTag("Player") && other.GetComponent<PlayerInventory>().hasKey2)
        {
            isPlayerInRange = true;
            hasKey2 = true;
        }

    }

   
}
