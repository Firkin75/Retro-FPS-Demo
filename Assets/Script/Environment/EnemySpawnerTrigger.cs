using UnityEngine;

public class EnemySpawnerTrigger : MonoBehaviour
{
    public GameObject enemySet; // The group of enemies to activate

    // Triggered when something enters the collider
    void OnTriggerEnter(Collider other)
    {
        // Only react to the player entering the trigger
        if (other.CompareTag("Player"))
        {
            enemySet.SetActive(true);  // Activate the enemy group
            Destroy(gameObject);       // Destroy this trigger to prevent re-activation
        }
        else
        {
            return; // Ignore all other objects
        }
    }
}
