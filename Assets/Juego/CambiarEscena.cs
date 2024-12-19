using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    [Header("Configuraci�n")]
    public string nombreEscena; // Nombre de la escena a cargar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveTest.saveTestNow = true;
            CambiarEscenas();
        }
    }

    private void CambiarEscenas()
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogError("No se ha configurado un nombre de escena v�lido en el script.");
        }
    }
}
