using UnityEngine;

public class GanchoInteractivo : MonoBehaviour
{
    public bool puedeSerAtraido = true;

    public void SerAtraido(Vector3 posicionJugador, float velocidadAtraer)
    {
        if (!puedeSerAtraido) return;

        Debug.Log($"{gameObject.name} está siendo atraído hacia el jugador.");
        StartCoroutine(MoverHaciaJugador(posicionJugador, velocidadAtraer));
    }

    private System.Collections.IEnumerator MoverHaciaJugador(Vector3 posicionJugador, float velocidadAtraer)
    {
        while (Vector3.Distance(transform.position, posicionJugador) > 1f)
        {
            Vector3 direccion = (posicionJugador - transform.position).normalized;
            transform.position += direccion * velocidadAtraer * Time.deltaTime;
            yield return null;
        }
        Debug.Log($"{gameObject.name} llegó al jugador.");
    }
}
