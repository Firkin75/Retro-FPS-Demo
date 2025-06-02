using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public bool isPlayerAlive = true;      // Whether the player is alive
    public static int maxHealth = 100;     // Maximum health
    public static int currentHealth;       // Current health (shared globally)

    public Text hpText;                    // UI Text to display HP
    public GameObject ui;                  // Reference to in-game UI to disable on death
    public AudioSource hitAudio;          // Audio source for getting hit

    [SerializeField]
    private float hitSoundCooldown = 2f;   // Cooldown for hit sound effect
    private float lastHitTime = -1f;       // Last time the hit sound was played

    void Start()
    {
        currentHealth = maxHealth; // Initialize HP
    }

    void Update()
    {
        // Update HP text UI every frame
        hpText.text = currentHealth.ToString();
    }

    // Called when the player takes damage
    public void TakeDamage(int damage)
    {
        int remainingDamage = damage;

        // Play hit sound if cooldown has passed
        if (Time.time - lastHitTime >= hitSoundCooldown)
        {
            hitAudio.Play();
            lastHitTime = Time.time;
        }

        // Apply damage if any
        if (remainingDamage > 0)
        {
            currentHealth -= remainingDamage;

            // Check if the player has died
            if (currentHealth <= 0)
            {
                Die();
                ui.SetActive(false); // Disable UI on death
            }
        }
    }

    // Called when healing or picking up health
    public void AddHealth(int amount)
    {
        // Heal but do not exceed max HP
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    // Handle player death
    private void Die()
    {
        Debug.Log("Player has died.");
        SceneManager.LoadScene(2); // Load death/game over scene
    }
}
