using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int amountOfHealth = 30; // Amount of health restored when picked up

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            // If player is already at full health, do nothing
            if (PlayerHealth.currentHealth >= PlayerHealth.maxHealth)
            {
                return;
            }

            // Restore health and display a pickup message
            health.AddHealth(amountOfHealth);
            PickUpMessage.OnPickupMessage?.Invoke($"You picked up a medic kit (hp + {amountOfHealth})");

            // Destroy the health pickup object
            Destroy(gameObject);
        }
    }
}
