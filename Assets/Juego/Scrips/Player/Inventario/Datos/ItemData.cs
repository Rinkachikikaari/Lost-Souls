using UnityEngine;

public enum ItemCategory { Objeto }
public enum ItemSubCategory { Pocion, Llave, Otro }

[CreateAssetMenu(fileName = "NuevoItem", menuName = "Inventario/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemCategory category;
    public ItemSubCategory subCategory;
    public string description;
}
