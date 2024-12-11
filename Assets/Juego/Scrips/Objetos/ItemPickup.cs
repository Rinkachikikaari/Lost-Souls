using System.Linq;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;
    public int cantidad;
    private bool EntreAntes = false;

    public bool AutoDestroyifInInventory = false;

    private void Update()
    {
        if (AutoDestroyifInInventory) { 
            if (InventoryManager.instance.items.Contains(this.itemData))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !EntreAntes)
        {
            EntreAntes = true;
            InventoryManager.instance.AddItem(itemData, cantidad);
            Destroy(gameObject); // Eliminar el objeto del mundo
        }
    }
}
