using UnityEngine;

public class FragmentosCorazon : MonoBehaviour
{
    [Header("Configuraci�n")]
    public int fragmentosActuales = 0; // Cantidad de fragmentos recolectados
    public int fragmentosNecesarios = 4; // Fragmentos para obtener un coraz�n completo

    private VidaJugador vidaJugador;

    private void Start()
    {
        // Obtener referencia al sistema de vida del jugador
        vidaJugador = GetComponent<VidaJugador>();
    }

    public void AgregarFragmento()
    {
        fragmentosActuales++;
        Debug.Log($"Fragmento obtenido. Total: {fragmentosActuales}/{fragmentosNecesarios}");

        if (fragmentosActuales >= fragmentosNecesarios)
        {
            AumentarVidaMaxima();
            fragmentosActuales = 0; // Reiniciar contador de fragmentos
        }
    }

    private void AumentarVidaMaxima()
    {
        vidaJugador.vidaMaxima += 4; // Un coraz�n completo equivale a 2 unidades de vida
        vidaJugador.CurarVida(vidaJugador.vidaMaxima); // Restaurar toda la vida
        Debug.Log("�Vida m�xima aumentada!");
    }
}
