using UnityEngine;

public class AKPickup : MonoBehaviour
{
  
    public string weaponName = "AK";  // ��Ҫʰȡ���������ƣ������ `WeaponHolder` �����������һ�£�
    private int slotIndex = 1;              // **�̶����� slot 3**

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
                Destroy(gameObject); // ʰȡ������ʰȡ��
                PickUpMessage.OnPickupMessage?.Invoke("You picked up a rifle");
            }
        }
    }
}
