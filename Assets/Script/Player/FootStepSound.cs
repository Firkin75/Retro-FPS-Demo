using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepAudioSource; // 脚步声的AudioSource
    public float minSpeed = 0.1f; // 最小速度阈值
    public float footstepDelay = 0.5f; // 脚步声播放间隔

    private CharacterController characterController;
    private Vector3 previousPosition;
    private float currentSpeed;
    private float timeSinceLastFootstep;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (footstepAudioSource == null)
        {
            Debug.LogError("AudioSource未分配！");
        }

        previousPosition = transform.position; // 初始化上一帧的位置
        timeSinceLastFootstep = footstepDelay; // 初始化脚步声计时器
    }

    void Update()
    {
        // 计算当前速度
        Vector3 positionDelta = transform.position - previousPosition;
        currentSpeed = positionDelta.magnitude / Time.deltaTime;
        previousPosition = transform.position;

        // 如果玩家在地面上且有速度
        if (characterController.isGrounded && currentSpeed > minSpeed)
        {
            timeSinceLastFootstep += Time.deltaTime;

            // 播放脚步声
            if (timeSinceLastFootstep >= footstepDelay)
            {
                PlayFootstep();
                timeSinceLastFootstep = 0f; // 重置计时器
            }
        }
        else
        {
            // 如果玩家停止移动，停止播放脚步声
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