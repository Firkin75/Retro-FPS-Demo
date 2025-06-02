using UnityEngine;

public class AKPickup : MonoBehaviour
{
  
    public string weaponName;  
    private int slotIndex = 1; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            WeaponManager weaponManager = FindFirstObjectByType<WeaponManager>();
            if (weaponManager != null)
            {
                GlobalAmmo.heavyAmmo += 10;
                weaponManager.PickupWeapon(weaponName,slotIndex);
                weaponManager.pickupSound.Play();
                Destroy(gameObject); 
                PickUpMessage.OnPickupMessage?.Invoke("You picked up a rifle");
            }
        }
    }
}
