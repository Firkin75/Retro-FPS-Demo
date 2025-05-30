using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // 确保正确找到主相机
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
        if (cameraTransform == null) return; // 避免空引用错误

        // 计算方向向量（让对象始终面对相机）
        Vector3 direction = cameraTransform.position - transform.position;

        // 如果不想让对象随相机上下旋转，可以锁定 y 轴：
        direction.y = 0;

        // 旋转对象，使其面向相机
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
