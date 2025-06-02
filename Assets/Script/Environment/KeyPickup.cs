using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public bool isKey1;
    public bool isKey2;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isKey1)
            {
                other.GetComponent<PlayerInventory>().hasKey1 = true;
                PickUpMessage.OnPickupMessage?.Invoke("You picked up the lab key");
            }
            if (isKey2)
            { 
                other.GetComponent<PlayerInventory>().hasKey2 = true;
                PickUpMessage.OnPickupMessage?.Invoke("You picked up the key for the door");
            }

            Destroy(gameObject);
        
        }
    }
}
