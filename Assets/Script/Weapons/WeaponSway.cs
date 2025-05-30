using UnityEngine;
using UnityEngine.UI;

public class WeaponSway : MonoBehaviour
{
    public float bobbingSpeed = 5f;  // 摆动速度
    public float bobbingAmount = 5f; // 摆动幅度

    private CharacterController playerController; // 角色控制器
    private RectTransform weaponUI;  // UI 组件的 RectTransform
    private Vector3 originalPosition;
    private Vector3 lastPosition; // 记录上一帧位置
    private float timer = 0f;
  

    void Start()
    {
        weaponUI = GetComponent<RectTransform>(); // 获取 UI 组件
        originalPosition = weaponUI.anchoredPosition; // 记录初始位置

        if (playerController == null)
        {
            playerController = FindFirstObjectByType<CharacterController>(); // 自动获取角色控制器
        }
        lastPosition = playerController.transform.position; // 记录初始位置
    }

    void Update()
    {
        if (playerController == null) return; // 防止报错

        // **用位置变化计算速度**
        float playerSpeed = (playerController.transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = playerController.transform.position; // 更新上一帧位置

        if (playerSpeed > 0.01f) // 设定一个较低的阈值
        {
            timer += Time.deltaTime * bobbingSpeed;
            float offsetX = Mathf.Sin(timer) * bobbingAmount; // 左右摆动
            float offsetY = Mathf.Cos(timer * 2) * bobbingAmount * 0.5f; // 上下摆动
            weaponUI.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
        }
        else
        {
            timer = 0; // 停止移动时归位
            weaponUI.anchoredPosition = Vector3.Lerp(weaponUI.anchoredPosition, originalPosition, Time.deltaTime * 5f);
        }
    }
}
