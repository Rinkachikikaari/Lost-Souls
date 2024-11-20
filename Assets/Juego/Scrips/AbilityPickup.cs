using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public AbilityData AbilityData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            InventoryManager.instance.UnlockAbility(AbilityData);
            Destroy(gameObject); // Eliminar el objeto del mundo
        }
    }
}
