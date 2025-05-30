using UnityEngine;

public class AKPickup : MonoBehaviour
{
  
    public string weaponName = "AK";  // 需要拾取的武器名称（必须和 `WeaponHolder` 里的武器名称一致）
    private int slotIndex = 1;              // **固定放入 slot 3**

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            WeaponManager weaponManager = FindFirstObjectByType<WeaponManager>();
            if (weaponManager != null)
            {
                GlobalAmmo.heavyAmmo += 20;
                weaponManager.PickupWeapon(weaponName,slotIndex);
                weaponManager.pickupSound.Play();
                Destroy(gameObject); // 拾取后销毁拾取物
                PickUpMessage.OnPickupMessage?.Invoke("You picked up a rifle");
            }
        }
    }
}
