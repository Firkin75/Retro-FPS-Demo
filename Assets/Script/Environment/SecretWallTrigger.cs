using UnityEngine;

public class SecretWallTrigger : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private bool hasOpened = false;
    private bool isMoving = false;

    [SerializeField] private float moveDistance = 1.5f;
    [SerializeField] private float moveSpeed = 1f;

    private Transform wall; // ǽ�屾�壨������
    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        wall = transform.parent; // ���败������ǽ��������
        closedPosition = wall.position;

        // �����󷽡��ƶ��������� -forward��������ĺ󷽷���
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
