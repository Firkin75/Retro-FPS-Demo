using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepAudioSource; // �Ų�����AudioSource
    public float minSpeed = 0.1f; // ��С�ٶ���ֵ
    public float footstepDelay = 0.5f; // �Ų������ż��

    private CharacterController characterController;
    private Vector3 previousPosition;
    private float currentSpeed;
    private float timeSinceLastFootstep;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (footstepAudioSource == null)
        {
            Debug.LogError("AudioSourceδ���䣡");
        }

        previousPosition = transform.position; // ��ʼ����һ֡��λ��
        timeSinceLastFootstep = footstepDelay; // ��ʼ���Ų�����ʱ��
    }

    void Update()
    {
        // ���㵱ǰ�ٶ�
        Vector3 positionDelta = transform.position - previousPosition;
        currentSpeed = positionDelta.magnitude / Time.deltaTime;
        previousPosition = transform.position;

        // �������ڵ����������ٶ�
        if (characterController.isGrounded && currentSpeed > minSpeed)
        {
            timeSinceLastFootstep += Time.deltaTime;

            // ���ŽŲ���
            if (timeSinceLastFootstep >= footstepDelay)
            {
                PlayFootstep();
                timeSinceLastFootstep = 0f; // ���ü�ʱ��
            }
        }
        else
        {
            // ������ֹͣ�ƶ���ֹͣ���ŽŲ���
            if (footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
        }
    }

    void PlayFootstep()
    {
        if (!footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Play();
        }
    }
}