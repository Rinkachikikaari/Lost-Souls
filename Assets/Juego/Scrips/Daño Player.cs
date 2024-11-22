using UnityEngine;

public class DañoPlayer : MonoBehaviour
{
    public int daño = 1; // Cantidad de vida que quita el enemigo
    public float tiempoEntreGolpes = 1f; // Tiempo de espera entre golpes

    private float tiempoUltimoGolpe = -Mathf.Infinity; // Registro del tiempo del último golpe

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtener el componente de vida del jugador
            VidaJugador vidaJugador = other.GetComponent<VidaJugador>();
            if (vidaJugador != null && PuedeGolpear())
            {
                vidaJugador.RecibirDano(daño); // Aplicar daño al jugador
                tiempoUltimoGolpe = Time.time; // Registrar el tiempo del golpe
            }
        }
    }

    private bool PuedeGolpear()
    {
        // Verifica si ha pasado suficiente tiempo desde el último golpe
        return Time.time >= tiempoUltimoGolpe + tiempoEntreGolpes;
    }
}
