using UnityEngine;

public class EquipPickup : MonoBehaviour
{
    public EquipmentData equipmentData;
    private bool EntreAntes = false;

    private void Update()
    {
        if (InventoryManager.instance.equipment.Contains(this.equipmentData))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !EntreAntes)
        {
            EntreAntes = true;
            InventoryManager.instance.AddEquipment(equipmentData);
            Destroy(gameObject); // Eliminar el objeto del mundo
        }
    }
}
