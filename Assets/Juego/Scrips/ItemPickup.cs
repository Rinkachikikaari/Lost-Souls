using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //InventoryManager.instance.AddItem("item");
            Destroy(gameObject); // Eliminar el objeto del mundo
        }
    }
}
