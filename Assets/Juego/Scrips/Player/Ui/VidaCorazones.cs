using UnityEngine;
using UnityEngine.UI;

public class VidaCorazones : MonoBehaviour
{
    [Header("Sprites de corazones")]
    public Sprite corazonLleno;
    public Sprite corazonTresCuartos;
    public Sprite corazonMedio;
    public Sprite corazonUnCuarto;
    public Sprite corazonVacio;

    [Header("Configuración de UI")]
    public GameObject contenedorCorazones;
    public GameObject prefabCorazon;

    [Header("Configuración dinámica")]
    public int corazonesPorFila = 10;
    public float escalaBase = 1f;
    public float reduccionEscala = 0.1f;

    [Header("Referencias al jugador")]
    private int vidaMaxima; // En corazones completos
    private int vidaActual; // En cuartos
    public VidaJugador uiCorazones;

    private Image[] corazonesUI;

    private int numCorazones;

    private void Start()
    {
        // Obtener referencia al sistema de Vida del jugador
        vidaMaxima = uiCorazones.vidaMaxima; // Convertir cuartos a corazones
        vidaActual = uiCorazones.vidaActual;
        if (contenedorCorazones == null || prefabCorazon == null)
        {
            Debug.LogError("Faltan referencias en el script VidaCorazones.");
            return;
        }

        GenerarCorazones();
        ActualizarCorazones();
    }
    private void Update()
    {
        // Obtener referencia al sistema de Vida del jugador
        vidaMaxima = uiCorazones.vidaMaxima; // Convertir cuartos a corazones
        vidaActual = uiCorazones.vidaActual;
        ActualizarCorazones();
    }
    public void GenerarCorazonesNuevos()
    {
        foreach (Transform child in contenedorCorazones.transform)
        {
            Destroy(child.gameObject);
        }
        numCorazones ++;
        corazonesUI = new Image[numCorazones];

        for (int i = 0; i < numCorazones; i++)
        {
            GameObject nuevoCorazon = Instantiate(prefabCorazon, contenedorCorazones.transform);
            if (nuevoCorazon != null)
            {
                corazonesUI[i] = nuevoCorazon.GetComponent<Image>();
                if (corazonesUI[i] == null)
                {
                    Debug.LogError("El prefab del corazón no tiene un componente Image.");
                }
            }
            else
            {
                Debug.LogError("No se pudo instanciar el prefab del corazón.");
            }
        }


        // Mantiene intacta la lógica actual de actualización
        ActualizarCorazones();
    }
    public void GenerarCorazones()
    {
        // Limpiar corazones anteriores
        foreach (Transform child in contenedorCorazones.transform)
        {
            Destroy(child.gameObject);
        }

        numCorazones = (vidaMaxima / 4); // Corazones completos
        corazonesUI = new Image[numCorazones];
        Debug.Log($"Generando {numCorazones} corazones.");

        for (int i = 0; i < numCorazones; i++)
        {
            GameObject nuevoCorazon = Instantiate(prefabCorazon, contenedorCorazones.transform);
            if (nuevoCorazon != null)
            {
                corazonesUI[i] = nuevoCorazon.GetComponent<Image>();
                if (corazonesUI[i] == null)
                {
                    Debug.LogError("El prefab del corazón no tiene un componente Image.");
                }
            }
            else
            {
                Debug.LogError("No se pudo instanciar el prefab del corazón.");
            }
        }

        AjustarEscalaYOrganizacion();
    }

    private void AjustarEscalaYOrganizacion()
    {
        int filas = Mathf.CeilToInt((float)vidaMaxima / (corazonesPorFila * 4)); // Cada fila puede tener corazonesPorFila completos
        float escala = escalaBase - (reduccionEscala * (filas - 1));
        escala = Mathf.Max(0.5f, escala);

        GridLayoutGroup layout = contenedorCorazones.GetComponent<GridLayoutGroup>();
        if (layout != null)
        {
            layout.constraintCount = corazonesPorFila;
            layout.cellSize = new Vector2(50f * escala, 50f * escala);
        }
        else
        {
            Debug.LogError("El contenedor de corazones no tiene un componente GridLayoutGroup.");
        }

        foreach (Image corazon in corazonesUI)
        {
            if (corazon != null)
            {
                corazon.transform.localScale = Vector3.one * escala;
            }
        }
    }

    public void ActualizarCorazones()
    {
        for (int i = 0; i < corazonesUI.Length; i++)
        {
            int vidaEnCorazon = Mathf.Clamp(vidaActual - (i * 4), 0, 4);

            switch (vidaEnCorazon)
            {
                case 4:
                    corazonesUI[i].sprite = corazonLleno;
                    break;
                case 3:
                    corazonesUI[i].sprite = corazonTresCuartos;
                    break;
                case 2:
                    corazonesUI[i].sprite = corazonMedio;
                    break;
                case 1:
                    corazonesUI[i].sprite = corazonUnCuarto;
                    break;
                default:
                    corazonesUI[i].sprite = corazonVacio;
                    break;
            }
        }
    }
}
