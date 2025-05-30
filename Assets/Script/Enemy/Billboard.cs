using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // ȷ����ȷ�ҵ������
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("Billboard: No main camera found!");
        }
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return; // ��������ô���

        // ���㷽���������ö���ʼ����������
        Vector3 direction = cameraTransform.position - transform.position;

        // ��������ö��������������ת���������� y �᣺
        direction.y = 0;

        // ��ת����ʹ���������
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
