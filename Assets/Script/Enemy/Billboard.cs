using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform; // Reference to the main camera's transform

    void Start()
    {
        // Ensure the main camera is found correctly
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
        if (cameraTransform == null) return; // Avoid null reference errors

        // Calculate direction vector from this object to the camera
        Vector3 direction = cameraTransform.position - transform.position;

        // Optional: Lock vertical rotation (for horizontal-only facing, e.g. 2.5D sprites)
        direction.y = 0;

        // Rotate the object to face the camera
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
