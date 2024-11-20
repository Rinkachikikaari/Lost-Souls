using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; // Singleton para acceso global

    public List<ItemData> items = new List<ItemData>();  // Lista de objetos
    public List<Equipment> equippedItems = new List<Equipment>();  // Equipados
    public List<AbilityData> abilities = new List<AbilityData>(); // Habilidades desbloqueadas

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(ItemData newItem)
    {
        if (!items.Contains(newItem))
        {
            items.Add(newItem);
            Debug.Log($"Añadido: {newItem.itemName}");
        }
    }

    public void EquipItem(Equipment equipment)
    {
        if (equipment != null && items.Contains(equipment))
        {
            equippedItems.Add(equipment);
            Debug.Log($"Equipado: {equipment.itemName}");
        }
    }

    public void UnlockAbility(AbilityData ability)
    {
        if (ability != null && !ability.isUnlocked)
        {
            ability.isUnlocked = true;
            abilities.Add(ability);
            Debug.Log($"Habilidad desbloqueada: {ability.itemName}");
        }
    }

    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.itemName == itemName);
    }

    public bool IsAbilityUnlocked(string abilityName)
    {
        return abilities.Exists(ability => ability.itemName == abilityName && ability.isUnlocked);
    }
}
