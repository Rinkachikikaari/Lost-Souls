using UnityEngine;

public enum EquipmentCategory { Arma, Armadura, Herramientas }
public enum EquipmentSubCategory { Espada, Arcos, Herramientas, Zapatos }

[CreateAssetMenu(fileName = "NuevoEquipo", menuName = "Inventario/Equipamiento")]
public class EquipmentData : ScriptableObject
{
    public string EquipName;
    public Sprite icon;
    public EquipmentCategory category;
    public EquipmentSubCategory subCategory;
    public int Daño;
    public int CostoDeStaminaMin;
    public int CostoDeStaminaMax;
    public int DuracionDeAtaqueBase;
    public int DuracionDeAtaqueMax;
    public int TiempoDeCarga;
    public int VelocidadMin;
    public int VelocidadMax;
    public bool SubManiro;
    public bool TeHundes;
    public bool cambiable; // Indica si el objeto puede ser cambiado
}
