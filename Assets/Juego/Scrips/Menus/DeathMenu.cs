using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    // M�todo para reintentar la escena actual
    public void Reintentar()
    {
        Debug.Log("Reiniciando la escena...");
        SceneManager.LoadScene("EscenaPrincipal");
    }

    // M�todo para salir al men� principal
    public void SalirAlMenu()
    {
        Debug.Log("Volviendo al men� principal...");
        SceneManager.LoadScene("Inicio"); // Aseg�rate de usar el nombre exacto de la escena del men� principal
    }

    // M�todo para salir del juego
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
