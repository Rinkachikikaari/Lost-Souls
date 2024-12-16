using NUnit.Framework.Interfaces;
using SH;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CurrentGameData : AbstractType
{
    [Header("Inventario")]
    public List<string> items = new List<string>();
    public List<int> cantidaditems = new List<int>();
    public List<string> Equip = new List<string>();
    public List<string> Ability = new List<string>();

    [Header("Equipado")]
    public string EspadaEquipada;
    public string ArcoEquipado;
    public string HerramientaEquipada;

    [Header("Stats")]
    public int VidaMaxima;
    public int VidaActual;
    public float Stamina; 
    public float StaminaActual;
    public float Mana;
    public float ManaActual;
    public int FragmentosDeCorazon;

    [Header("Posicion y Escena")]
    public string EscenaActual;
    public float HoraActual;
    public float PosX;
    public float PosY;
    public float PosZ;



}

public class SaveTest : MonoBehaviour
{
    [SerializeField] bool saveTestNow;
    [SerializeField] bool loadTestNow;
    [SerializeField] bool removeDataNow;
    public CurrentGameData testDataToSave = new CurrentGameData();
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerMain");
        LoadData();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(saveTestNow)
        {
            SaveGame();
        }
        if (loadTestNow)
        {
            LoadData();
        }
        if (removeDataNow)
        {
            removeDataNow = false;
            SH.Save saveConfig = new SH.Save() { encode = false, fileName = "GameData", onResources = false };
            StaticSaveLoad.RemoveFile(saveConfig);
        }
        
    }
    public void SaveGame()
    {
        saveTestNow = false;
        testDataToSave.items.Clear();
        testDataToSave.cantidaditems.Clear();
        testDataToSave.Equip.Clear();
        testDataToSave.Ability.Clear();

        testDataToSave.HoraActual = CicloDiaNoche.tiempoActual;

        testDataToSave.VidaMaxima = VidaJugador.instance.vidaMaxima;
        testDataToSave.VidaActual = VidaJugador.instance.vidaActual;
        testDataToSave.Stamina = EstaminaJugador.instance.estaminaMaxima;
        testDataToSave.StaminaActual = EstaminaJugador.instance.estaminaActual;
        testDataToSave.Mana = Magia.instance.manaMaxima;
        testDataToSave.ManaActual = Magia.instance.manaActual;
        testDataToSave.FragmentosDeCorazon = FragmentosCorazon.instance.fragmentosActuales;

        if(PlayerStats.instance.espadaEquipada != null)
        {
            testDataToSave.EspadaEquipada = PlayerStats.instance.espadaEquipada.name;

        }
        if (PlayerStats.instance.HerramientaActiva != null)
        {
            testDataToSave.HerramientaEquipada = PlayerStats.instance.HerramientaActiva.name;

        }
        if (PlayerStats.instance.arcoEquipado != null)
        {
            testDataToSave.ArcoEquipado = PlayerStats.instance.arcoEquipado.name;

        }

        testDataToSave.PosX = player.transform.position.x;
        testDataToSave.PosZ = player.transform.position.z;
        testDataToSave.PosY = player.transform.position.y;

        testDataToSave.EscenaActual = SceneManager.GetActiveScene().name;


        foreach (var item in InventoryManager.instance.items)
        {
            testDataToSave.items.Add(item.name);
            testDataToSave.cantidaditems.Add(item.cantidad);
            print(item.ToString());
        }
        foreach (var equipment in InventoryManager.instance.equipment)
        {
            testDataToSave.Equip.Add(equipment.name);
            print(equipment.ToString());
        }
        foreach (var abilities in InventoryManager.instance.abilities)
        {
            testDataToSave.Ability.Add(abilities.name);
            print(abilities.ToString());
        }

        // Now save the main Game Data :3
        SH.Save saveConfig = new SH.Save() { encode = false, fileName = "GameData", onResources = false };
        List<CurrentGameData> gd = new List<CurrentGameData>() { testDataToSave };
        SH.StaticSaveLoad.Save(gd, saveConfig);
    }

    public void LoadData()
    {
        
        loadTestNow = false;
        SH.Save saveConfig = new SH.Save() { encode = false, fileName = "GameData", onResources = false };
        bool exist = SH.StaticSaveLoad.CheckIfSaveExist(saveConfig);
        if(StaticSaveLoad.CheckIfSaveExist(saveConfig) == false)
        {
            return;
        }

        testDataToSave = SH.StaticSaveLoad.Load<CurrentGameData>(saveConfig)[0];

        if(SceneManager.GetActiveScene().name == "Inicio")
        {
            MainMenu.instance.EscenaGuardada = testDataToSave.EscenaActual;

        }
        if (SceneManager.GetActiveScene().name == "Muerto")
        {
            DeathMenu.instance.EscenaGuardada = testDataToSave.EscenaActual;

        }

        if (player != null)
        {
            for (int i = 0; i < testDataToSave.items.Count; i++)
            {
                print("SO/" + testDataToSave.items[i].ToString());
                ItemData itemData = Resources.Load<ItemData>("SO/" + testDataToSave.items[i]);
                InventoryManager.instance.AddItem(itemData, testDataToSave.cantidaditems[i]);
            }

            for (int i = 0; i < testDataToSave.Equip.Count; i++)
            {
                print("SO/" + testDataToSave.Equip[i].ToString());
                EquipmentData EquipmentData = Resources.Load<EquipmentData>("SO/" + testDataToSave.Equip[i]);
                InventoryManager.instance.AddEquipment(EquipmentData);
            }

            for (int i = 0; i < testDataToSave.Ability.Count; i++)
            {
                print("SO/" + testDataToSave.Ability[i].ToString());
                AbilityData AbilityData = Resources.Load<AbilityData>("SO/" + testDataToSave.Ability[i]);
                InventoryManager.instance.UnlockAbility(AbilityData);
            }

            VidaJugador.instance.vidaMaxima = testDataToSave.VidaMaxima;
            VidaJugador.instance.vidaActual = testDataToSave.VidaActual;
            EstaminaJugador.instance.estaminaMaxima = testDataToSave.Stamina;
            EstaminaJugador.instance.estaminaActual = testDataToSave.StaminaActual;
            Magia.instance.manaMaxima = testDataToSave.Mana;
            Magia.instance.manaActual = testDataToSave.ManaActual;
            FragmentosCorazon.instance.fragmentosActuales = testDataToSave.FragmentosDeCorazon;

            EquipmentData Espada = Resources.Load<EquipmentData>("SO/" + testDataToSave.EspadaEquipada);
            PlayerStats.instance.espadaEquipada = Espada;

            EquipmentData Arco = Resources.Load<EquipmentData>("SO/" + testDataToSave.ArcoEquipado);
            PlayerStats.instance.arcoEquipado = Arco;

            ItemData Herramienta = Resources.Load<ItemData>("SO/" + testDataToSave.HerramientaEquipada);
            PlayerStats.instance.HerramientaActiva = Herramienta;

            if (testDataToSave.EscenaActual == SceneManager.GetActiveScene().name)
            {
                Vector3 nuevaPosicion = player.transform.position;
                nuevaPosicion.y = testDataToSave.PosY;
                nuevaPosicion.x = testDataToSave.PosX;
                nuevaPosicion.z = testDataToSave.PosZ;

                player.transform.position = nuevaPosicion;

            }

            CicloDiaNoche.tiempoActual = testDataToSave.HoraActual;
        }

    }
}
