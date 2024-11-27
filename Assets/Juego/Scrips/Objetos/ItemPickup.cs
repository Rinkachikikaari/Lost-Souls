using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;
    public int cantidad;
    private bool EntreAntes = false;

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
