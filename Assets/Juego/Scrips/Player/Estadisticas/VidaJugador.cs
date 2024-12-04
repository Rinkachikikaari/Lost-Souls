using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement; // Importar para manejo de escenas

public class VidaJugador : MonoBehaviour
{
    [Header("Configuración de vida")]
    public int vidaMaxima; // 12 unidades de vida (3 corazones)
    public int vidaActual; // Vida inicial en cuartos
    public VidaCorazones VidaCorazones;

    [Header("Inmunidad al Daño")]
    public float duracionInmunidad = 0.5f; // Duración de la inmunidad en segundos
    private bool esInmune = false; // Indica si el jugador está en estado de inmunidad


    private Animator animator;
    private void Start()
    {
        vidaActual = vidaMaxima; // Asegura que la vida esté completa al inicio
        animator = GetComponent<Animator>();    

    }

    public void CurarVida(int cantidad)
    {
        if (vidaActual != vidaMaxima)
        {
            if (vidaActual + cantidad < vidaMaxima)
            {
                vidaActual = vidaActual + cantidad;
            }
        }
    }

    public void RecibirDano(int cantidad)
    {
        if (esInmune)
        {
            Debug.Log("El jugador es inmune al daño.");
            return; // Ignorar el daño si está en estado inmune
        }

        Debug.Log($"Recibiendo daño: {cantidad}");
        if (vidaActual - cantidad >= 0)
        {
            vidaActual -= cantidad;
        }
        else
        {
            vidaActual = 0;
        }

        if (vidaActual <= 0)
        {
            Muerte();
        }
        else
        {
            IniciarInmunidad(); // Activar la inmunidad tras recibir daño
        }
    }

    private void IniciarInmunidad()
    {
        esInmune = true;
        Invoke(nameof(TerminarInmunidad), duracionInmunidad); // Terminar la inmunidad después de 0.5 segundos
    }

    private void TerminarInmunidad()
    {
        esInmune = false;
        Debug.Log("El jugador ya no es inmune.");
    }

    private void Muerte()
    {
        Debug.Log("El jugador ha muerto.");
        animator.SetTrigger("Muerte");
    }

    public void AumentarVidaMaxima(int cantidadCuartos)
    {
        vidaMaxima += cantidadCuartos;
        VidaCorazones.GenerarCorazonesNuevos();

        if (vidaActual < vidaMaxima)
        {
            vidaActual = vidaMaxima;
        }
    }
    public void Destruirse()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Muerte"); // Reemplaza "EscenaDeMuerte" con el nombre real de la escena
    }
}
