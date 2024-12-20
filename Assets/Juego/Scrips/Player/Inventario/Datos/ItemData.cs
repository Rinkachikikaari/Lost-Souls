using UnityEngine;

public enum ItemCategory { Objeto }
public enum ItemSubCategory { Pocion, Llave, ObjetoEspecial, Otro, Moneda, Bombas }


[CreateAssetMenu(fileName = "NuevoItem", menuName = "Inventario/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemCategory category;
    public ItemSubCategory subCategory;
    public int cantidad;         // Cantidad del �tem (si es acumulable)
    public bool esAcumulable;
    public bool esCambiale;
    public int Curacion;
    public int precio;
    public string description;
}
