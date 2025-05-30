using UnityEngine;

public class PickupHGAmmo : MonoBehaviour
{
    public int supplyAmount;
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GlobalAmmo.PickUpPistolAmmo(supplyAmount);
            PickUpMessage.OnPickupMessage?.Invoke($"You picked up pistol ammo£¨+{supplyAmount}£©");
            Destroy(gameObject);
        }
    }
}
