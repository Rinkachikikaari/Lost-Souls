using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    [Header("Configuraci�n de vida")]
    public int vidaMaxima; // 12 unidades de vida (3 corazones)
    public int vidaActual; // Vida inicial en cuartos
    public VidaCorazones VidaCorazones;




    private void Start()
    {
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
        vidaActual = Mathf.Clamp(vidaActual - cantidad, 0, vidaMaxima);


        if (vidaActual <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        Debug.Log("El jugador ha muerto.");
        // Implementa la l�gica de muerte aqu�
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
}
