using UnityEngine;
using UnityEngine.UI;

public class WeaponSway : MonoBehaviour
{
    public float bobbingSpeed = 5f;  // �ڶ��ٶ�
    public float bobbingAmount = 5f; // �ڶ�����

    private CharacterController playerController; // ��ɫ������
    private RectTransform weaponUI;  // UI ����� RectTransform
    private Vector3 originalPosition;
    private Vector3 lastPosition; // ��¼��һ֡λ��
    private float timer = 0f;
  

    void Start()
    {
        weaponUI = GetComponent<RectTransform>(); // ��ȡ UI ���
        originalPosition = weaponUI.anchoredPosition; // ��¼��ʼλ��

        if (playerController == null)
        {
            playerController = FindFirstObjectByType<CharacterController>(); // �Զ���ȡ��ɫ������
        }
        lastPosition = playerController.transform.position; // ��¼��ʼλ��
    }

    void Update()
    {
        if (playerController == null) return; // ��ֹ����

        // **��λ�ñ仯�����ٶ�**
        float playerSpeed = (playerController.transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = playerController.transform.position; // ������һ֡λ��

        if (playerSpeed > 0.01f) // �趨һ���ϵ͵���ֵ
        {
            timer += Time.deltaTime * bobbingSpeed;
            float offsetX = Mathf.Sin(timer) * bobbingAmount; // ���Ұڶ�
            float offsetY = Mathf.Cos(timer * 2) * bobbingAmount * 0.5f; // ���°ڶ�
            weaponUI.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
        }
        else
        {
            timer = 0; // ֹͣ�ƶ�ʱ��λ
            weaponUI.anchoredPosition = Vector3.Lerp(weaponUI.anchoredPosition, originalPosition, Time.deltaTime * 5f);
        }
    }
}
