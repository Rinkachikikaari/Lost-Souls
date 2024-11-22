using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;
    private bool EntreAntes = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !EntreAntes)
        {
            EntreAntes = true;
            InventoryManager.instance.AddItem(itemData);
            Destroy(gameObject); // Eliminar el objeto del mundo
        }
    }
}
