using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryUI; // Referencia al GameObject del inventario
    public static bool isInventoryOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && PauseMenu.GameIsPaused == false)
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Time.timeScale = 0f; // Pausa el juego
            Debug.Log("Inventario abierto.");
        }
        else
        {
            Time.timeScale = 1f; // Reanuda el juego
            Debug.Log("Inventario cerrado.");
        }
    }
}
