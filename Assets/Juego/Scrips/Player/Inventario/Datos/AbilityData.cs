using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Ability")]
public class AbilityData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    public int manaCost;
}
