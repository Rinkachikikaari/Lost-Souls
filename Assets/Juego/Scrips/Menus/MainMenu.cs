using SH;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    public string EscenaGuardada;

    [Header("Referencias UI")]
    public Button botonJugar; // Asigna el bot�n desde el Inspector
    public Image botonJugarI; // Asigna el bot�n desde el Inspector

    private void Awake()
    {
        instance = this;
        ActualizarEstadoBotonJugar();
    }
    private void Update()
    {
        ActualizarEstadoBotonJugar();
    }

    private void ActualizarEstadoBotonJugar()
    {
        if (string.IsNullOrEmpty(EscenaGuardada))
        {
            botonJugar.interactable = false; // Desactivar el bot�n
            // botonJugarI.color.WithAlpha(225); // Hacerlo semitransparente
        }
        else
        {
            botonJugar.interactable = true; // Activar el bot�n
            // botonJugarI.color.WithAlpha(100); // Hacerlo semitransparente
        }
    }
    // M�todo para iniciar el juego
    public void Jugar()
    {
        // Cambiar a la escena principal del juego
        SceneManager.LoadScene(EscenaGuardada); // Reemplaza con el nombre de tu escena de juego
    }

    public void NewGame()
    {
        SH.Save saveConfig = new SH.Save() { encode = false, fileName = "GameData", onResources = false };
        StaticSaveLoad.RemoveFile(saveConfig);
        SceneManager.LoadScene("Tutorial");
    }

    // M�todo para abrir las opciones
    public void AbrirOpciones()
    {
        Debug.Log("Abriendo el men� de opciones...");
        // Aqu� podr�as habilitar un panel de opciones si ya est� configurado
    }

    // M�todo para salir del juego
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
