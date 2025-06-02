using UnityEngine;

public class MGPickup : MonoBehaviour
{
    public string weaponName = "MG";  // Name of the weapon to assign
    private int slotIndex = 2;        // The inventory slot index for this weapon

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Try to find the WeaponManager instance in the scene
            WeaponManager weaponManager = FindFirstObjectByType<WeaponManager>();
            if (weaponManager != null)
            {
                // Add ammo for the MG weapon
                GlobalAmmo.heavyAmmo += 10;

                // Register the weapon pickup in the WeaponManager
                weaponManager.PickupWeapon(weaponName, slotIndex);

                // Play pickup sound
                weaponManager.pickupSound.Play();

                // Destroy the pickup object after being collected
                Destroy(gameObject);

                // Display pickup message using event
                PickUpMessage.OnPickupMessage?.Invoke("You picked up a chain gun");
            }
        }
    }
}
