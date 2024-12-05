using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // M�todo para iniciar el juego
    public void Jugar()
    {
        // Cambiar a la escena principal del juego
        SceneManager.LoadScene("EscenaPrincipal"); // Reemplaza con el nombre de tu escena de juego
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
