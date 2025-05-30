using UnityEngine;

public class P08Pickup : MonoBehaviour
    

{
   
    private string weaponName = "P08";  // ��Ҫʰȡ���������ƣ������ `WeaponHolder` �����������һ�£�
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

               

                
                Destroy(gameObject); // ʰȡ������ʰȡ��
            }
        }
    }
}
