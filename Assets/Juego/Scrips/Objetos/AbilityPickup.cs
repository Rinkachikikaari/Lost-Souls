using System.Linq;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public AbilityData AbilityData;
    private bool EntreAntes = false;

    private void Update()
    {
        if (InventoryManager.instance.abilities.Contains(this.AbilityData))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !EntreAntes)
        {
            EntreAntes = true;
            InventoryManager.instance.UnlockAbility(AbilityData);
            Destroy(gameObject); // Eliminar el objeto del mundo
        }
    }
}
