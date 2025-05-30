using UnityEngine;

public class MGPickup : MonoBehaviour
{

    public string weaponName = "MG";  // ��Ҫʰȡ���������ƣ������ `WeaponHolder` �����������һ�£�
    private int slotIndex = 2;              // **�̶����� slot 3**

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
                Destroy(gameObject); // ʰȡ������ʰȡ��
                PickUpMessage.OnPickupMessage?.Invoke("You picked up a chain gun");
            }
        }
    }
}
