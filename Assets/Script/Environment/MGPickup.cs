using UnityEngine;

public class MGPickup : MonoBehaviour
{

    public string weaponName = "MG";  // 需要拾取的武器名称（必须和 `WeaponHolder` 里的武器名称一致）
    private int slotIndex = 2;              // **固定放入 slot 3**

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            WeaponManager weaponManager = FindFirstObjectByType<WeaponManager>();
            if (weaponManager != null)
            {
                GlobalAmmo.heavyAmmo += 30;
                weaponManager.PickupWeapon(weaponName, slotIndex);
                weaponManager.pickupSound.Play();
                Destroy(gameObject); // 拾取后销毁拾取物
                PickUpMessage.OnPickupMessage?.Invoke("You picked up a chain gun");
            }
        }
    }
}
