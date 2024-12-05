using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    // Método para reintentar la escena actual
    public void Reintentar()
    {
        Debug.Log("Reiniciando la escena...");
        SceneManager.LoadScene("EscenaPrincipal");
    }

    // Método para salir al menú principal
    public void SalirAlMenu()
    {
        Debug.Log("Volviendo al menú principal...");
        SceneManager.LoadScene("Inicio"); // Asegúrate de usar el nombre exacto de la escena del menú principal
    }

    // Método para salir del juego
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
