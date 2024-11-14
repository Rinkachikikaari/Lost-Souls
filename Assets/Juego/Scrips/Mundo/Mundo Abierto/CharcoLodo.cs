using UnityEngine;

public class CharcoLodo : MonoBehaviour
{
    public float reduccionVelocidad = 0.5f; // Reducción de velocidad (50% en este ejemplo)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Movimiento movimientoJugador = other.GetComponent<Movimiento>();
            if (movimientoJugador != null)
            {
                movimientoJugador.speed *= reduccionVelocidad;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Movimiento movimientoJugador = other.GetComponent<Movimiento>();
            if (movimientoJugador != null)
            {
                movimientoJugador.speed /= reduccionVelocidad;
            }
        }
    }
}
