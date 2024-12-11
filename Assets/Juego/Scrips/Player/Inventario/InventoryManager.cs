
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; // Singleton para acceso global

    public List<ItemData> items = new List<ItemData>();
    public List<EquipmentData> equipment = new List<EquipmentData>();
    public List<AbilityData> abilities = new List<AbilityData>();
    private Dictionary<EquipmentCategory, EquipmentData> equippedItems = new Dictionary<EquipmentCategory, EquipmentData>();
    private HashSet<EquipmentSubCategory> equippedCategories = new HashSet<EquipmentSubCategory>(); // Usado para asegurar que un equipo se equipe automáticamente solo la primera vez.
    private Dictionary<ItemCategory, ItemData> ItemsEquip = new Dictionary<ItemCategory, ItemData>();
    private HashSet<ItemSubCategory> itemCategory = new HashSet<ItemSubCategory>();

    private void Awake()
    {
        instance = this;
        
    }
    public void UsarItem(ItemData item)
    {
        if (PuedoUsarItem(item))
        {
            item.cantidad--;
            Debug.Log($"Usaste {item.itemName}. Cantidad restante: {item.cantidad}");
            InventoryUI.instance.UpdateInventoryUI(); // Actualiza la UI inmediatamente
        }
        else
        {
            Debug.Log($"No puedes usar {item.itemName}.");
        }
    }

    public bool PuedoUsarItem(ItemData item)
    {
        return item.esAcumulable && item.cantidad > 0;
    }

    public void AddItem(ItemData newItem, int cantidad)
    {
        if (newItem.esAcumulable)
        {
            ItemData itemExistente = items.Find(i => i.itemName == newItem.itemName);
            if (itemExistente != null)
            {
                itemExistente.cantidad += cantidad;
                InventoryUI.instance.UpdateInventoryUI(); // Actualiza la UI inmediatamente

            }
            else
            {
                newItem.cantidad = cantidad;
                items.Add(newItem);
                InventoryUI.instance.UpdateInventoryUI(); // Actualiza la UI inmediatamente

            }
            if (!IsSubCategoryItem(newItem.subCategory) && newItem.subCategory != ItemSubCategory.Moneda && newItem.subCategory != ItemSubCategory.Llave && newItem.subCategory != ItemSubCategory.Otro)
            {
                ItemEquip(newItem);
                Debug.Log($"Equipamiento automático: {newItem.itemName}");

                // Llamar al método adecuado en PlayerStats para actualizar el equipo actual
                if (newItem.subCategory == ItemSubCategory.Pocion)
                {
                    PlayerStats.instance.EquiparHerramienta(newItem);
                }
                else if (newItem.subCategory == ItemSubCategory.Bombas)
                {
                    PlayerStats.instance.EquiparHerramienta(newItem);
                }
                else if (newItem.subCategory == ItemSubCategory.ObjetoEspecial)
                {
                    PlayerStats.instance.EquiparHerramienta(newItem);
                }
            }
        }
        else if (!items.Contains(newItem))
        {
            items.Add(newItem);
            Debug.Log($"Añadido: {newItem.itemName}");
            InventoryUI.instance.UpdateInventoryUI(); // Actualiza la UI inmediatamente
            if (!IsSubCategoryItem(newItem.subCategory) && newItem.subCategory != ItemSubCategory.Moneda && newItem.subCategory != ItemSubCategory.Llave && newItem.subCategory != ItemSubCategory.Otro)
            {
                ItemEquip(newItem);
                Debug.Log($"Equipamiento automático: {newItem.itemName}");

                // Llamar al método adecuado en PlayerStats para actualizar el equipo actual
                if (newItem.subCategory == ItemSubCategory.Pocion)
                {
                    PlayerStats.instance.EquiparHerramienta(newItem);
                }
                else if (newItem.subCategory == ItemSubCategory.Bombas)
                {
                    PlayerStats.instance.EquiparHerramienta(newItem);
                }
                else if (newItem.subCategory == ItemSubCategory.ObjetoEspecial)
                {
                    PlayerStats.instance.EquiparHerramienta(newItem);
                }
            }

        }
    }

    public void AddEquipment(EquipmentData equip)
    {
        if (!equipment.Contains(equip))
        {
            equipment.Add(equip);
            Debug.Log($"Equipamiento agregado: {equip.EquipName}");
            InventoryUI.instance.UpdateInventoryUI(); // Actualiza la UI inmediatamente


            // Verificar si no hay un objeto equipado de esta subcategoría
            if (!IsSubCategoryEquipped(equip.subCategory))
            {
                EquipItem(equip);
                Debug.Log($"Equipamiento automático: {equip.EquipName}");

                // Llamar al método adecuado en PlayerStats para actualizar el equipo actual
                if (equip.subCategory == EquipmentSubCategory.Espada)
                {
                    PlayerStats.instance.EquiparEspada(equip);
                }
                else if (equip.subCategory == EquipmentSubCategory.Arcos)
                {
                    PlayerStats.instance.EquiparArco(equip);
                }
                else if (equip.subCategory == EquipmentSubCategory.Zapatos)
                {
                    PlayerStats.instance.EquiparZapatos(equip);
                }
            }
        }
    }



    public void UnlockAbility(AbilityData ability)
    {
        if (ability != null)
        {
            abilities.Add(ability);
            Debug.Log($"Habilidad desbloqueada: {ability.abilityName}");
            InventoryUI.instance.UpdateInventoryUI(); // Actualiza la UI inmediatamente

        }
    }

    public bool EquipItem(EquipmentData newEquipment)
    {
        if (!newEquipment.cambiable)
        {
            Debug.LogWarning($"{newEquipment.EquipName} no se puede equipar.");
            return false;
        }

        equippedItems[newEquipment.category] = newEquipment;
        Debug.Log($"Equipado: {newEquipment.EquipName} en categoría {newEquipment.category}");
        return true;
    }

    public bool ItemEquip(ItemData NewItemEquip)
    {
        if (!NewItemEquip.esCambiale)
        {
            Debug.LogWarning($"{NewItemEquip.itemName} no se puede equipar.");
            return false;
        }

        ItemsEquip[NewItemEquip.category] = NewItemEquip;
        Debug.Log($"Equipado: {NewItemEquip.itemName} en categoría {NewItemEquip.category}");
        return true;
    }

    public void UnequipItem(EquipmentCategory category)
    {
        if (equippedItems.ContainsKey(category))
        {
            Debug.Log($"Desquipado: {equippedItems[category].EquipName} de categoría {category}");
            equippedItems.Remove(category);
        }
    }

    public EquipmentData GetEquippedItem(EquipmentCategory category)
    {
        equippedItems.TryGetValue(category, out EquipmentData equipped);
        return equipped;
    }

    public List<ItemData> GetItemsBySubCategory(ItemSubCategory subCategory)
    {
        return items.FindAll(item => item.subCategory == subCategory);
    }

    public List<EquipmentData> GetEquipmentBySubCategory(EquipmentSubCategory subCategory)
    {
        return equipment.FindAll(equip => equip.subCategory == subCategory);
    }

    public List<AbilityData> GetAbilitiesBySubCategory(AbilitySubCategory subCategory)
    {
        return abilities.FindAll(ability => ability.subCategory == subCategory);
    }

    // Verificar si ya hay un objeto equipado de la subcategoría
    private bool IsSubCategoryEquipped(EquipmentSubCategory subCategory)
    {
        foreach (var equipped in equippedItems.Values)
        {
            if (equipped.subCategory == subCategory)
                return true;
        }
        return false;
    }
    private bool IsSubCategoryItem(ItemSubCategory subCategory)
    {
        foreach (var Item in ItemsEquip.Values)
        {
            if (Item.subCategory == subCategory)
                return true;
        }
        return false;
    }
    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.itemName == itemName);
    }

    public bool IsAbilityUnlocked(string abilityName)
    {
        return abilities.Exists(ability => ability.abilityName == abilityName);
    }

    public bool HasEquip(string EquipName)
    {
        return equipment.Exists(equip => equip.EquipName == EquipName);
    }
}
