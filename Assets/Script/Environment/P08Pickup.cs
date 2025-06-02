using UnityEngine;

public class P08Pickup : MonoBehaviour
{
    private string weaponName = "P08";   // Name of the weapon to register in WeaponManager
    private int slotIndex = 1;           // Slot in which to place the weapon

    void Start()
    {
        // (Optional) Initialization logic can go here
    }

    void OnTriggerEnter(Collider other)
    {
        // Only respond to the player
        if (other.CompareTag("Player"))
        {
            WeaponManager weaponManager = FindFirstObjectByType<WeaponManager>();
            if (weaponManager != null)
            {
                // Add handgun ammo
                GlobalAmmo.handGunAmmo += 4;

                // Play pickup sound
                weaponManager.pickupSound.Play();

                // Register weapon into the player's inventory
                weaponManager.PickupWeapon(weaponName, slotIndex);

                // Destroy the pickup object after collection
                Destroy(gameObject);

                // Display pickup message
                PickUpMessage.OnPickupMessage?.Invoke("You picked up a pistol (ammo +4)");
            }
        }
    }
}
