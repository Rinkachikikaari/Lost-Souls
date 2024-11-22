using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public Transform espadasGrid;
    public Transform arcosGrid;
    public Transform zapatosGrid;
    public Transform herramientasGrid;
    public Transform magiaGrid;
    public Transform tecnicaGrid;
    public Transform pocionesGrid;
    public Transform llavesGrid;
    public Transform otrosGrid;

    public GameObject itemSlotPrefab;

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

    private void OnEnable()
    {
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        // Limpia los grids
        ClearGrid(espadasGrid);
        ClearGrid(arcosGrid);
        ClearGrid(zapatosGrid);
        ClearGrid(herramientasGrid);
        ClearGrid(magiaGrid);
        ClearGrid(tecnicaGrid);
        ClearGrid(pocionesGrid);
        ClearGrid(llavesGrid);
        ClearGrid(otrosGrid);

        // Actualiza los grids con los ítems correspondientes
        foreach (var equip in InventoryManager.instance.equipment)
        {
            switch (equip.subCategory)
            {
                case EquipmentSubCategory.Espada:
                    CreateItemSlot(espadasGrid, equip, () => PlayerStats.instance.EquiparEspada(equip));
                    break;
                case EquipmentSubCategory.Arcos:
                    CreateItemSlot(arcosGrid, equip, () => PlayerStats.instance.EquiparArco(equip));
                    break;
                case EquipmentSubCategory.Zapatos:
                    CreateItemSlot(zapatosGrid, equip, () => PlayerStats.instance.EquiparZapatos(equip));
                    break;
                case EquipmentSubCategory.Herramientas:
                    CreateItemSlot(herramientasGrid, equip, null); // Herramientas no tienen lógica especial por ahora
                    break;
                default:
                    Debug.Log($"Subcategoría desconocida: {equip.subCategory}");
                    break;
            }
        }

        foreach (var ability in InventoryManager.instance.abilities)
        {
            switch (ability.subCategory)
            {
                case AbilitySubCategory.Magia:
                    CreateItemSlot(magiaGrid, ability, null); // Habilidades no se equipan directamente
                    break;
                case AbilitySubCategory.Tecnica:
                    CreateItemSlot(tecnicaGrid, ability, null);
                    break;
                default:
                    Debug.Log($"Subcategoría desconocida: {ability.subCategory}");
                    break;
            }
        }

        foreach (var item in InventoryManager.instance.items)
        {
            switch (item.subCategory)
            {
                case ItemSubCategory.Pocion:
                    CreateItemSlot(pocionesGrid, item, null);
                    break;
                case ItemSubCategory.Llave:
                    CreateItemSlot(llavesGrid, item, null);
                    break;
                case ItemSubCategory.Otro:
                    CreateItemSlot(otrosGrid, item, null);
                    break;
                default:
                    Debug.Log($"Subcategoría desconocida: {item.subCategory}");
                    break;
            }
        }
    }

    private void CreateItemSlot(Transform parentGrid, ScriptableObject itemData, System.Action onClickAction)
    {
        GameObject slot = Instantiate(itemSlotPrefab, parentGrid);
        Image icon = slot.GetComponentInChildren<Image>();
        if (itemData is EquipmentData equipment)
        {
            icon.sprite = equipment.icon;
        }
        else if (itemData is AbilityData ability)
        {
            icon.sprite = ability.icon;
        }
        else if (itemData is ItemData item)
        {
            icon.sprite = item.icon;
        }

        if (onClickAction != null)
        {
            Button button = slot.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClickAction.Invoke());
        }
    }

    private void ClearGrid(Transform grid)
    {
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
    }
}
