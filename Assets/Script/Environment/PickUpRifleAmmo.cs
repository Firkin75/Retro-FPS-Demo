using UnityEngine;

public class PickUpRifleAmmo : MonoBehaviour
{
    public int supplyAmount; // Amount of rifle ammo to add when picked up

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Add rifle ammo using GlobalAmmo system
            GlobalAmmo.PickUpRifleAmmo(supplyAmount);

            // Show pickup message on screen
            PickUpMessage.OnPickupMessage?.Invoke($"You picked up rifle ammo (+{supplyAmount})");

            // Destroy this pickup object after being collected
            Destroy(gameObject);
        }
    }
}
