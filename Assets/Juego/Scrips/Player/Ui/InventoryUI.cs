using TMPro;
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
    public Transform bombasGrid;
    public Transform monedasGrid;
    public Transform otrosGrid;

    public GameObject monedaSlotPrefab; // Prefab exclusivo para monedas
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
    private void Start()
    {
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        if (InventoryManager.instance == null)
        {
            Debug.LogWarning("InventoryManager no está inicializado.");
            return;
        }
        // Limpia los grids
        ClearGrid(espadasGrid);
        ClearGrid(arcosGrid);
        ClearGrid(zapatosGrid);
        ClearGrid(herramientasGrid);
        ClearGrid(magiaGrid);
        ClearGrid(tecnicaGrid);
        ClearGrid(pocionesGrid);
        ClearGrid(llavesGrid);
        ClearGrid(bombasGrid);
        ClearGrid(otrosGrid);
        ClearGrid(monedasGrid);

        if (InventoryManager.instance.equipment == null || InventoryManager.instance.abilities == null || InventoryManager.instance.items == null)
        {
            Debug.LogWarning("Alguna lista de InventoryManager es nula.");
            return;
        }
        // Actualiza los grids con los ítems correspondientes
        foreach (var equip in InventoryManager.instance.equipment)
        {
            if (equip == null) continue;
            switch (equip.subCategory)
            {
                case EquipmentSubCategory.Espada:
                    CreateItemSlot(espadasGrid, equip, () => PlayerStats.instance.EquiparEspada(equip), itemSlotPrefab);
                    break;
                case EquipmentSubCategory.Arcos:
                    CreateItemSlot(arcosGrid, equip, () => PlayerStats.instance.EquiparArco(equip), itemSlotPrefab);
                    break;
                case EquipmentSubCategory.Zapatos:
                    CreateItemSlot(zapatosGrid, equip, () => PlayerStats.instance.EquiparZapatos(equip), itemSlotPrefab);
                    break;
                case EquipmentSubCategory.Herramientas:
                    CreateItemSlot(herramientasGrid, equip, null, itemSlotPrefab); // Herramientas no tienen lógica especial por ahora
                    break;
                default:
                    Debug.Log($"Subcategoría desconocida: {equip.subCategory}");
                    break;
            }
        }

        foreach (var ability in InventoryManager.instance.abilities)
        {
            if (ability == null) continue;

            switch (ability.subCategory)
            {
                case AbilitySubCategory.Magia:
                    CreateItemSlot(magiaGrid, ability, null, itemSlotPrefab); // Habilidades no se equipan directamente
                    break;
                case AbilitySubCategory.Tecnica:
                    CreateItemSlot(tecnicaGrid, ability, null, itemSlotPrefab);
                    break;
                default:
                    Debug.Log($"Subcategoría desconocida: {ability.subCategory}");
                    break;
            }
        }

        foreach (var item in InventoryManager.instance.items)
        {
            if (item == null) continue;


            switch (item.subCategory)
            {
                case ItemSubCategory.Pocion:
                    CreateItemSlot(pocionesGrid, item, null, itemSlotPrefab);
                    break;
                case ItemSubCategory.Llave:
                    CreateItemSlot(llavesGrid, item, null, itemSlotPrefab);
                    break;
                case ItemSubCategory.Bombas:
                    CreateItemSlot(otrosGrid, item, null, itemSlotPrefab);
                    break;
                case ItemSubCategory.Moneda:
                    CreateItemSlot(monedasGrid, item, null, monedaSlotPrefab);
                    break;
                case ItemSubCategory.Otro:
                    CreateItemSlot(otrosGrid, item, null, itemSlotPrefab);
                    break;
                default:
                    Debug.Log($"Subcategoría desconocida: {item.subCategory}");
                    break;
            }
        }
    }

    private void CreateItemSlot(Transform parentGrid, ScriptableObject itemData, System.Action onClickAction, GameObject prefab)
    {
        if (itemData == null || parentGrid == null || prefab == null)
        {
            Debug.LogWarning("Faltan datos en CreateItemSlot: itemData, parentGrid o prefab son nulos.");
            return;
        }

        GameObject slot = Instantiate(prefab, parentGrid);

        Image icon = slot.GetComponentInChildren<Image>();
        TextMeshProUGUI cantidadTexto = slot.transform.Find("Cantidad")?.GetComponent<TextMeshProUGUI>();

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
            if (cantidadTexto != null)
            {
                cantidadTexto.text = item.esAcumulable ? item.cantidad.ToString() : "";
            }
            else
            {
                Debug.LogWarning($"El slot para {item.itemName} no tiene un componente 'Cantidad'.");
            }
        }

        if (onClickAction != null)
        {
            Button button = slot.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => onClickAction.Invoke());
            }
            else
            {
                Debug.LogWarning($"El slot para {itemData.name} no tiene un componente Button.");
            }
        }
    }


    private void ClearGrid(Transform grid)
    {
        if (grid == null)
        {
            Debug.LogWarning("Intentando limpiar un grid nulo.");
            return;
        }

        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
    }
}
