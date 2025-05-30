using UnityEngine;

public class PickUpRifleAmmo : MonoBehaviour 

{
    public int supplyAmount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GlobalAmmo.PickUpPistolAmmo(supplyAmount);
            PickUpMessage.OnPickupMessage?.Invoke($"You picked up rifle ammo£¨+{supplyAmount}£©");
            Destroy(gameObject);
        }
    }
}
