using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            InventoryManager.instance.AddItem(itemData);
            Destroy(gameObject); // Eliminar el objeto del mundo
        }
    }
}
