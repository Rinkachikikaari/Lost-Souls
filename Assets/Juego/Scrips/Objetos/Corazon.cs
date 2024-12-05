using UnityEngine;

public class Corazon : MonoBehaviour
{
    public int cantidadCuraMin; // Cantidad de vida que este corazón restaura
    public int cantidadCuraMax;
    private int cantidadCura;
    private bool Recogido = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Recogido)
        {
            VidaJugador vidaJugador = other.GetComponentInParent<VidaJugador>();
            if (vidaJugador != null)
            {
                Recogido=true;
                int cantidadCura = Random.Range(cantidadCuraMin, cantidadCuraMax);
                vidaJugador.CurarVida(cantidadCura);
                Debug.Log("Jugador curado por " + cantidadCura + " puntos de vida.");
                Destroy(gameObject); // Destruir el corazón después de recogerlo
            }
        }
    }
}
