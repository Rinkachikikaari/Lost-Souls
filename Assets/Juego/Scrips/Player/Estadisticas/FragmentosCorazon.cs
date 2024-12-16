using UnityEngine;

public class FragmentosCorazon : MonoBehaviour
{
    public static FragmentosCorazon instance; // Singleton para acceso global

    [Header("Configuraci�n")]
    public int fragmentosActuales = 0; // Cantidad de fragmentos recolectados
    public int fragmentosNecesarios = 4; // Fragmentos para obtener un coraz�n completo

    private VidaJugador vidaJugador;

    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        // Obtener referencia al sistema de vida del jugador
        vidaJugador = GetComponent<VidaJugador>();
    }

    public void AgregarFragmento(int cantidad)
    {
        fragmentosActuales += cantidad;
        Debug.Log($"Fragmento obtenido. Total: {fragmentosActuales}/{fragmentosNecesarios}");

        while (fragmentosActuales >= fragmentosNecesarios)
        {
            AumentarVidaMaxima();
            fragmentosActuales -= fragmentosNecesarios; // Reducir fragmentos restantes
        }
    }

    private void AumentarVidaMaxima()
    {
        vidaJugador.AumentarVidaMaxima(4); // Un coraz�n completo equivale a 2 unidades de vida
        Debug.Log("�Vida m�xima aumentada!");
    }
}
