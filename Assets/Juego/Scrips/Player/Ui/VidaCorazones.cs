using UnityEngine;
using UnityEngine.UI;

public class VidaCorazones : MonoBehaviour
{
    [Header("Sprites de corazones")]
    public Sprite corazonLleno;
    public Sprite corazonMedio;
    public Sprite corazonVacio;

    [Header("Configuración de UI")]
    public GameObject contenedorCorazones;
    public GameObject prefabCorazon;

    [Header("Referencias al jugador")]
    public int vidaMaxima = 10;
    public int vidaActual = 10;

    private Image[] corazonesUI;

    private void Start()
    {
        GenerarCorazones();
        ActualizarCorazones();
    }

    private void GenerarCorazones()
    {
        // Crear los corazones en la UI según la vida máxima
        corazonesUI = new Image[vidaMaxima];
        for (int i = 0; i < vidaMaxima; i++)
        {
            GameObject nuevoCorazon = Instantiate(prefabCorazon, contenedorCorazones.transform);
            corazonesUI[i] = nuevoCorazon.GetComponent<Image>();
        }
    }

    public void ActualizarCorazones()
    {
        for (int i = 0; i < corazonesUI.Length; i++)
        {
            if (i < vidaActual / 2)
            {
                corazonesUI[i].sprite = corazonLleno;
            }
            else if (i == vidaActual / 2 && vidaActual % 2 != 0)
            {
                corazonesUI[i].sprite = corazonMedio;
            }
            else
            {
                corazonesUI[i].sprite = corazonVacio;
            }
        }
    }

    public void CambiarVida(int cantidad)
    {
        vidaActual = Mathf.Clamp(vidaActual + cantidad, 0, vidaMaxima);
        ActualizarCorazones();
    }
}
