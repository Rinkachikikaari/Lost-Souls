using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemGrid;  // Contenedor de la cuadrícula
    public GameObject itemSlotPrefab;  // Prefab de cada celda

    private void Start()
    {
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);  // Limpiar la cuadrícula
        }

        foreach (Item item in InventoryManager.instance.items)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemGrid);
            slot.GetComponent<Image>().sprite = item.icon;
            slot.GetComponent<Button>().onClick.AddListener(() => SelectItem(item));
        }
    }

    private void SelectItem(Item item)
    {
        Debug.Log($"Seleccionado: {item.itemName}");
        // Lógica para equipar o usar el objeto
    }
}
