
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Ataque weaponManager;
    [SerializeField] private DisparoFlecha ArcosManager;

    public static PlayerStats instance;

    [Header("Equipamientos Actuales")]
    public EquipmentData espadaEquipada;
    public EquipmentData arcoEquipado;
    public EquipmentData zapatosEquipados;

    [Header("Herramientas Disponibles")]
    public List<EquipmentData> herramientasDisponibles = new List<EquipmentData>();

    [Header("Stats Totales")]
    public int DañoEspada;
    public int DañoArco;
    public int CostoDeStaminaMinArco;
    public int CostoDeStaminaMinEspada;
    public int CostoDeStaminaMaxArco;
    public int CostoDeStaminaMaxEspada;
    public int DuracionDeAtaqueBaseEspada;
    public int DuracionDeAtaqueMaxEspada;
    public int TiempoDeCargaArco;
    public int TiempoDeCargaEspada;
    public int VelocidadMinArco;
    public int VelocidadMaxArco;
    public int VelocidadMaxZapatos;
    public bool SubManiroArco;
    public bool TeHundes;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EquiparEspada(EquipmentData espada)
    {
        if (espada.subCategory == EquipmentSubCategory.Espada && espada.cambiable)
        {
            espadaEquipada = espada;
            ActualizarStats();
            // Llamar al método para actualizar currentWeapon
            if (weaponManager != null)
            {
                weaponManager.UpdateCurrentWeapon();
            }
        }
    }

    public void EquiparArco(EquipmentData arco)
    {
        if (arco.subCategory == EquipmentSubCategory.Arcos && arco.cambiable)
        {
            arcoEquipado = arco;
            ActualizarStats();
            // Llamar al método para actualizar currentWeapon
            if (weaponManager != null)
            {
                ArcosManager.UpdateCurrentWeapon();
            }
        }
    }

    public void EquiparZapatos(EquipmentData zapatos)
    {
        if (zapatos.subCategory == EquipmentSubCategory.Zapatos && zapatos.cambiable)
        {
            zapatosEquipados = zapatos;
            ActualizarStats();
        }
    }

    public void ActualizarStats()
    {
        DañoEspada = 0;
        DañoArco = 0;
        CostoDeStaminaMinEspada = 0;
        CostoDeStaminaMaxEspada = 0;
        CostoDeStaminaMinArco = 0;
        CostoDeStaminaMaxArco = 0;
        DuracionDeAtaqueBaseEspada = 0;
        DuracionDeAtaqueMaxEspada = 0;
        TiempoDeCargaEspada = 0;
        TiempoDeCargaArco = 0;
        VelocidadMinArco = 0;
        VelocidadMaxArco = 0;
        VelocidadMaxZapatos = 0;
        SubManiroArco = false;
        TeHundes = false;

        if (espadaEquipada != null)
        {
            DañoEspada += espadaEquipada.Daño;
            TiempoDeCargaEspada += espadaEquipada.TiempoDeCarga;
            CostoDeStaminaMinEspada += espadaEquipada.CostoDeStaminaMin;
            CostoDeStaminaMaxEspada += espadaEquipada.CostoDeStaminaMax;
            DuracionDeAtaqueBaseEspada += espadaEquipada.DuracionDeAtaqueBase;
            DuracionDeAtaqueMaxEspada += espadaEquipada.DuracionDeAtaqueMax;
        }

        if (arcoEquipado != null)
        {
            DañoArco += arcoEquipado.Daño;
            TiempoDeCargaArco += arcoEquipado.TiempoDeCarga;
            CostoDeStaminaMinArco += arcoEquipado.CostoDeStaminaMin;
            CostoDeStaminaMaxArco += arcoEquipado.CostoDeStaminaMax;
            VelocidadMinArco += arcoEquipado.VelocidadMin;
            VelocidadMaxArco += arcoEquipado.VelocidadMax;
            SubManiroArco = arcoEquipado.SubManiro;
        }

        if (zapatosEquipados != null)
        {
            VelocidadMaxZapatos = zapatosEquipados.VelocidadMax;
            TeHundes = zapatosEquipados.TeHundes;
        }

        Debug.Log($"Stats actualizados");
    }

    public void MostrarEquipamientos()
    {
        Debug.Log($"Espada equipada: {(espadaEquipada != null ? espadaEquipada.EquipName : "Ninguna")}");
        Debug.Log($"Arco equipado: {(arcoEquipado != null ? arcoEquipado.EquipName : "Ninguno")}");
        Debug.Log($"Zapatos equipados: {(zapatosEquipados != null ? zapatosEquipados.EquipName : "Ningunos")}");

        Debug.Log("Herramientas disponibles:");
        foreach (var herramienta in herramientasDisponibles)
        {
            Debug.Log($"- {herramienta.EquipName}");
        }
    }
}
