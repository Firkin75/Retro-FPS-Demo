using UnityEngine;
using UnityEngine.UI;

public class WeaponSway : MonoBehaviour
{
    public float bobbingSpeed = 5f;   // Speed of weapon sway
    public float bobbingAmount = 5f;  // Intensity of sway

    private CharacterController playerController;  // Reference to the player's CharacterController
    private RectTransform weaponUI;                // UI weapon element's RectTransform
    private Vector3 originalPosition;              // Original anchored position
    private Vector3 lastPosition;                  // Player's last position
    private float timer = 0f;                      // Timer to control sway animation

    void Start()
    {
        // Get RectTransform component of this weapon UI
        weaponUI = GetComponent<RectTransform>();
        originalPosition = weaponUI.anchoredPosition; // Store original UI position

        // Auto-find the player CharacterController
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<CharacterController>();
        }

        lastPosition = playerController.transform.position; // Initialize last position
    }

    void Update()
    {
        if (playerController == null) return; // Safety check

        // Calculate player movement speed based on position delta
        float playerSpeed = (playerController.transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = playerController.transform.position; // Update last position

        if (playerSpeed > 0.01f) // Apply sway if movement is significant
        {
            timer += Time.deltaTime * bobbingSpeed;

            // Horizontal sway using sine wave
            float offsetX = Mathf.Sin(timer) * bobbingAmount;

            // Vertical sway using faster sine wave, smaller amplitude
            float offsetY = Mathf.Cos(timer * 2) * bobbingAmount * 0.5f;

            // Apply sway to UI weapon position
            weaponUI.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
        }
        else
        {
            // Return to original position smoothly when idle
            timer = 0;
            weaponUI.anchoredPosition = Vector3.Lerp(weaponUI.anchoredPosition, originalPosition, Time.deltaTime * 5f);
        }
    }
}
