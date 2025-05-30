using UnityEngine;

public class P08Pickup : MonoBehaviour
    

{
   
    private string weaponName = "P08";  // 需要拾取的武器名称（必须和 `WeaponHolder` 里的武器名称一致）
    private int slotIndex = 1;


    void Start()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("weaponPickup");
            WeaponManager weaponManager = FindFirstObjectByType<WeaponManager>();
            if (weaponManager != null)
            {
                GlobalAmmo.handGunAmmo += 15;
                weaponManager.pickupSound.Play();
                weaponManager.PickupWeapon(weaponName, slotIndex);

               

                
                Destroy(gameObject); // 拾取后销毁拾取物
            }
        }
    }
}
