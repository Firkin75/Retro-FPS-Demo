using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;              // Player movement speed
    public float mouseSensitivity = 100f;     // Mouse sensitivity for camera rotation

    private CharacterController controller;   // CharacterController component for movement
    private Transform cameraTransform;        // Reference to the main camera
    private Vector3 velocity;                 // Player velocity (used for gravity)
    private float xRotation = 0f;             // Vertical camera rotation value
    private float gravity = 9.81f;            // Gravity force

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // ----------- Player Movement -----------
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down arrow

        // Move relative to the player's facing direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply movement
        controller.Move(move * walkSpeed * Time.deltaTime);

        // Apply gravity over time
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ----------- Mouse Movement -----------
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // Only horizontal (yaw) rotation is handled here; vertical (pitch) is fixed
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
