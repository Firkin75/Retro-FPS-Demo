using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepAudioSource; // AudioSource component for footstep sound
    public float minSpeed = 0.1f;           // Minimum speed threshold to trigger footstep sound
    public float footstepDelay = 0.5f;      // Delay between footstep sounds

    private CharacterController characterController;
    private Vector3 previousPosition;       // Position in the previous frame
    private float currentSpeed;             // Current movement speed
    private float timeSinceLastFootstep;    // Timer to track delay between footsteps

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (footstepAudioSource == null)
        {
            Debug.LogError("AudioSource is not assigned!");
        }

        previousPosition = transform.position;     // Initialize previous position
        timeSinceLastFootstep = footstepDelay;     // Initialize timer
    }

    void Update()
    {
        // Calculate movement speed based on position change
        Vector3 positionDelta = transform.position - previousPosition;
        currentSpeed = positionDelta.magnitude / Time.deltaTime;
        previousPosition = transform.position;

        // Check if player is grounded and moving above threshold
        if (characterController.isGrounded && currentSpeed > minSpeed)
        {
            timeSinceLastFootstep += Time.deltaTime;

            // Play footstep sound if enough time has passed
            if (timeSinceLastFootstep >= footstepDelay)
            {
                PlayFootstep();
                timeSinceLastFootstep = 0f; // Reset timer
            }
        }
        else
        {
            // Stop sound if player is not moving or not grounded
            if (footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
        }
    }

    // Play the footstep sound if not already playing
    void PlayFootstep()
    {
        if (!footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Play();
        }
    }
}
