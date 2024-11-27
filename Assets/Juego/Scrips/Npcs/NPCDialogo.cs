using UnityEngine;

public class NPCDialogo : MonoBehaviour
{
    [Header("Di�logo")]
    public string[] lineasDialogo; // L�neas del di�logo del NPC
    public GameObject panelDialogo; // Panel del di�logo en la UI
    public UnityEngine.UI.Text textoDialogo; // Campo de texto donde se muestra el di�logo

    private int indiceActual = 0; // �ndice de la l�nea actual del di�logo
    private bool enDialogo = false; // Si el di�logo est� activo
    private bool cercaDelNPC = false; // Si el jugador est� cerca del NPC

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cercaDelNPC && !enDialogo)
            {
                IniciarDialogo(); // Comienza el di�logo al presionar E
            }
            else if (enDialogo)
            {
                SiguienteLinea(); // Avanza en el di�logo al presionar E
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cercaDelNPC = true; // El jugador est� cerca del NPC
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cercaDelNPC = false; // El jugador se aleja del NPC
            if (enDialogo)
            {
                FinalizarDialogo(); // Finaliza el di�logo si el jugador se aleja
            }
        }
    }
}
