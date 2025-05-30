using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int amountOfHealth = 30;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {

            PlayerHealth health = other.GetComponent<PlayerHealth>();
            PickUpMessage.OnPickupMessage?.Invoke($"You picked up a medic kit (hp + {amountOfHealth})");
            health.AddHealth(amountOfHealth);
        
        }
        Destroy(gameObject);
    }
}
