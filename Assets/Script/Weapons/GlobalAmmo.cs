using UnityEngine;
using UnityEngine.UI;

public class GlobalAmmo : MonoBehaviour
{
    // Static variables to store global ammo counts
    public static int handGunAmmo;
    public static int heavyAmmo;
    public static string fist = "inf"; // Fist weapon has infinite ammo

    // Sound to play when ammo is picked up
    public AudioSource pickupSound;

    void Start()
    {
        // Initialize starting ammo values
        handGunAmmo = 15;
        heavyAmmo = 0;
    }

    // Increase handgun ammo and play pickup sound
    public static void PickUpPistolAmmo(int amount)
    {
        handGunAmmo += amount;
        PlayPickupSound();
    }

    // Increase rifle ammo and play pickup sound
    public static void PickUpRifleAmmo(int amount)
    {
        heavyAmmo += amount;
        PlayPickupSound();
    }

    // Increase machine gun ammo and play pickup sound
    public static void PickUpMGAmmo(int amount)
    {
        heavyAmmo += amount;
        PlayPickupSound();
    }

    // Play pickup sound using the instance of GlobalAmmo in the scene
    private static void PlayPickupSound()
    {
        // Find an instance of GlobalAmmo to access the AudioSource
        GlobalAmmo instance = FindFirstObjectByType<GlobalAmmo>();
        if (instance != null && instance.pickupSound != null)
        {
            instance.pickupSound.Play();
        }
    }
}
