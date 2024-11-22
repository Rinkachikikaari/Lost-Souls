using UnityEngine;

public enum AbilityCategory {Habilidad}
public enum AbilitySubCategory { Magia, Tecnica }

[CreateAssetMenu(fileName = "NuevaHabilidad", menuName = "Inventario/Habilidad")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    public AbilityCategory category;
    public AbilitySubCategory subCategory;
    public string description;
}
