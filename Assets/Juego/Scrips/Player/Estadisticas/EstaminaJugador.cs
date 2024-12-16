using UnityEngine;
using System.Collections;

public class EstaminaJugador : MonoBehaviour
{
    public static EstaminaJugador instance; // Singleton para acceso global


    public float estaminaMaxima = 100f;         // Máxima estamina del jugador
    public float tiempoRecarga = 7f;            // Tiempo sin usar estamina para comenzar la recarga
    public float tiempoRecargaCompleta = 3f;    // Tiempo para recargar estamina completamente

    public float estaminaActual;
    private float ultimoUsoTiempo;
    private Coroutine recargaCoroutine = null;  // Referencia a la coroutine de recarga

    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
        estaminaActual = estaminaMaxima;
    }

    void Update()
    {
        // Si no estamos en proceso de recarga y ha pasado el tiempo de inactividad
        if (recargaCoroutine == null && Time.time - ultimoUsoTiempo >= tiempoRecarga)
        {
            recargaCoroutine = StartCoroutine(RecargarEstamina());
        }
    }

    public bool UsarEstamina(float cantidad)
    {
        if (estaminaActual >= cantidad)
        {
            estaminaActual -= cantidad;
            ultimoUsoTiempo = Time.time;  // Actualizar el tiempo de último uso

            // Si está en proceso de recarga, detenerla
            if (recargaCoroutine != null)
            {
                StopCoroutine(recargaCoroutine);
                recargaCoroutine = null;
            }

            return true;
        }
        else
        {
            return false;  // No hay suficiente estamina
        }
    }

    private IEnumerator RecargarEstamina()
    {
        float estaminaInicial = estaminaActual;
        float tiempoInicioRecarga = Time.time;

        while (estaminaActual < estaminaMaxima && Time.time - tiempoInicioRecarga <= tiempoRecargaCompleta)
        {
            // Incrementa la estamina en el tiempo de recarga
            estaminaActual = Mathf.Lerp(estaminaInicial, estaminaMaxima, (Time.time - tiempoInicioRecarga) / tiempoRecargaCompleta);
            yield return null;

            // Verificar si se usó estamina durante la recarga para detener la recarga
            if (Time.time - ultimoUsoTiempo < tiempoRecarga)
            {
                recargaCoroutine = null;
                yield break;
            }
        }

        estaminaActual = estaminaMaxima;
        recargaCoroutine = null;
    }

    public float ObtenerEstaminaActual()
    {
        return estaminaActual;
    }
}
