using UnityEngine;

public class NPCDialogo : MonoBehaviour
{
    [Header("Configuración de Diálogo")]
    public string[] lineasDialogo; // Líneas del diálogo del NPC
    public GameObject panelDialogo; // Panel del diálogo en la UI
    public UnityEngine.UI.Text textoDialogo; // Campo de texto donde se muestra el diálogo
    public bool puedeRepetirDialogo = true; // Opción para permitir repetir el diálogo

    private int indiceActual = 0; // Índice de la línea actual del diálogo
    private bool enDialogo = false; // Si el diálogo está activo
    private bool cercaDelNPC = false; // Si el jugador está cerca del NPC
    private bool dialogoCompletado = false; // Controla si el diálogo ya se completó

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cercaDelNPC && !enDialogo && (!dialogoCompletado || puedeRepetirDialogo))
            {
                IniciarDialogo(); // Comienza el diálogo al presionar E
            }
            else if (enDialogo)
            {
                SiguienteLinea(); // Avanza en el diálogo al presionar E
            }
        }
    }

    public void IniciarDialogo()
    {
        if (lineasDialogo.Length > 0)
        {
            enDialogo = true;
            indiceActual = 0;
            panelDialogo.SetActive(true);
            textoDialogo.text = lineasDialogo[indiceActual];
        }
    }

    public void SiguienteLinea()
    {
        indiceActual++;
        if (indiceActual < lineasDialogo.Length)
        {
            textoDialogo.text = lineasDialogo[indiceActual];
        }
        else
        {
            FinalizarDialogo();
        }
    }

    public void FinalizarDialogo()
    {
        enDialogo = false;
        panelDialogo.SetActive(false);

        if (!puedeRepetirDialogo)
        {
            dialogoCompletado = true; // Marca el diálogo como completado solo si no se puede repetir
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cercaDelNPC = true; // El jugador está cerca del NPC
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cercaDelNPC = false; // El jugador se aleja del NPC
            if (enDialogo)
            {
                FinalizarDialogo(); // Finaliza el diálogo si el jugador se aleja
            }
        }
    }
}
