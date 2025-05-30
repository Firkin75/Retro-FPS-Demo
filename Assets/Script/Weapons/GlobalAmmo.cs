using UnityEngine;
using UnityEngine.UI;


public class GlobalAmmo : MonoBehaviour
{
    public static int handGunAmmo = 15;
    public static int heavyAmmo;
    public static string fist = "inf";
    public AudioSource pickupSound;





    public static void PickUpPistolAmmo(int amount)
    {
        handGunAmmo += amount;
        PlayPickupSound();
    }

    public static void PickUpRifleAmmo(int amount)
    {
        heavyAmmo += amount;
        PlayPickupSound();
    }

    public static void PickUpMGAmmo(int amount)
    {
        heavyAmmo += amount;
        PlayPickupSound();
    }

    private static void PlayPickupSound()
    {
        // ÕÒµ½ GlobalAmmo µÄÊµÀý
        GlobalAmmo instance = FindFirstObjectByType<GlobalAmmo>();
        if (instance != null && instance.pickupSound != null)
        {
            instance.pickupSound.Play();
        }
    }


}



