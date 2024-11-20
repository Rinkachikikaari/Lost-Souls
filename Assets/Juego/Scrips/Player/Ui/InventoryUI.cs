using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemGrid;  // Contenedor de la cuadrícula
    public GameObject itemSlotPrefab;  // Prefab de cada celda

    private void OnEnable()
    {
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);  // Limpiar la cuadrícula
        }

        foreach (ItemData item in InventoryManager.instance.items)
        {
            itemSlotPrefab.SetActive(true);
            GameObject slot = Instantiate(itemSlotPrefab, itemGrid);
            slot.GetComponent<Image>().sprite = item.icon;
            slot.GetComponent<Button>().onClick.AddListener(() => SelectItem(item));
            //itemSlotPrefab.SetActive(false); 
        }

        foreach (AbilityData item in InventoryManager.instance.abilities)
        {
            itemSlotPrefab.SetActive(true);
            GameObject slot = Instantiate(itemSlotPrefab, itemGrid);
            slot.GetComponent<Image>().sprite = item.icon;
            slot.GetComponent<Button>().onClick.AddListener(() => SelectAbility(item));
            //itemSlotPrefab.SetActive(false); 
        }
    }

    private void SelectItem(ItemData item)
    {
        Debug.Log($"Seleccionado: {item.itemName}");
        // Lógica para equipar o usar el objeto
    }

    private void SelectAbility(AbilityData item)
    {
        Debug.Log($"Seleccionado: {item.itemName}");
        // Lógica para equipar o usar el objeto
    }
}
