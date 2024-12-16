using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; // Estado de pausa del juego
    public GameObject pauseMenuUI;          // Referencia al men� de pausa en el Canvas
    public GameObject pauseAjustesUI;          // Referencia al men� de pausa en el Canvas


    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && InventoryToggle.isInventoryOpen == false) // Activar/Desactivar el men� con "Escape"
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);       // Ocultar el men� de pausa
        pauseAjustesUI.SetActive(false);
        Time.timeScale = 1f;               // Restaurar la velocidad normal del juego
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);        // Mostrar el men� de pausa
        Time.timeScale = 0f;               // Detener el tiempo del juego
        GameIsPaused = true;
    }

    public void LoadOptions()
    {
        Debug.Log("Cargando el men� de opciones...");
        pauseMenuUI.SetActive(false);
        pauseAjustesUI.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;               // Restaurar el tiempo del juego
        SceneManager.LoadScene("Inicio"); // Cambiar a la escena del men� principal
    }

}
