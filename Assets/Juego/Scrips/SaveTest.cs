using NUnit.Framework.Interfaces;
using SH;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class CurrentGameData : AbstractType
{
    public List<string> items = new List<string>();
    public List<int> cantidaditems = new List<int>();
    public List<string> Equip = new List<string>();
    public List<string> Ability = new List<string>();
}

public class SaveTest : MonoBehaviour
{
    [SerializeField] bool saveTestNow;
    [SerializeField] bool loadTestNow;
    [SerializeField] bool removeDataNow;
    public CurrentGameData testDataToSave = new CurrentGameData();
    

    // Start is called before the first frame update
    void Start()
    {
        
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        if(saveTestNow)
        {
            saveTestNow = false;
            testDataToSave.items.Clear();
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
        
    }
}
