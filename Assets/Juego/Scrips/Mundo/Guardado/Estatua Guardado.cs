using UnityEngine;
using UnityEngine.UI;

public class StatueSavePoint : MonoBehaviour
{
    [Header("Configuración del Guardado")]
    public GameObject dialogoUI; // Panel de UI para el diálogo
    public Text dialogoTexto;    // Texto del diálogo
    public string mensajeDialogo = "¿Seguro que quieres guardar tu progreso, aventurero? Presiona 'Y' para aceptar o 'N' para cancelar.";

    private bool jugadorCerca = false;

    void Start()
    {
        // Asegurarse de que el diálogo esté desactivado al inicio
        dialogoUI.SetActive(false);
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            // Mostrar el diálogo
            MostrarDialogo();
        }

        if (dialogoUI.activeSelf)
        {
            // Confirmar guardado con 'Y'
            if (Input.GetKeyDown(KeyCode.Y))
            {
                GuardarProgreso();
                CerrarDialogo();
            }

            // Cancelar con 'N'
            if (Input.GetKeyDown(KeyCode.N))
            {
                CerrarDialogo();
            }
        }
    }

    private void MostrarDialogo()
    {
        dialogoUI.SetActive(true);
        dialogoTexto.text = mensajeDialogo;
        Time.timeScale = 0f; // Pausar el tiempo mientras aparece el diálogo
    }

    private void CerrarDialogo()
    {
        dialogoUI.SetActive(false);
        Time.timeScale = 1f; // Reanudar el tiempo
    }

    private void GuardarProgreso()
    {
        SaveTest.saveTestNow = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}
