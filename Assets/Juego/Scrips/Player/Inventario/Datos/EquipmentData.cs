using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
public class EquipmentData : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    public int defense;
    public int attackBonus;
    public EquipmentSlot slot; // Enum para indicar si es casco, armadura, etc.
}

public enum EquipmentSlot
{
    Head,
    Body,
    Legs,
    Weapon
}

