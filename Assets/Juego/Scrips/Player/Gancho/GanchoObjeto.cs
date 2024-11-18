using UnityEngine;

public class GanchoObjeto : MonoBehaviour
{
    public float velocidadGancho = 20f;
    public float distanciaMaxima = 15f;

    private Vector3 direccion;
    private Transform jugador;
    private Gancho ganchoScript;

    private void Update()
    {
        transform.position += direccion * velocidadGancho * Time.deltaTime;

        if (Vector3.Distance(transform.position, jugador.position) > distanciaMaxima)
        {
            Debug.Log("Gancho alcanzó su distancia máxima.");
            ganchoScript.FinGancho();
            Destroy(gameObject);
        }
    }

    public void ConfigurarGancho(Vector3 dir, Transform jugadorRef, Gancho gancho)
    {
        direccion = dir.normalized;
        jugador = jugadorRef;
        ganchoScript = gancho;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemigo = other.GetComponent<Enemy>();
        if (enemigo != null && enemigo.puedeSerAtraido)
        {
            ganchoScript.AtraerObjetivo(other.transform);
            Destroy(gameObject);
            return;
        }

        GanchoInteractivo objetoInteractivo = other.GetComponent<GanchoInteractivo>();
        if (objetoInteractivo != null && objetoInteractivo.puedeSerAtraido)
        {
            ganchoScript.AtraerObjetivo(other.transform);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Superficie"))
        {
            ganchoScript.MoverHaciaSuperficie(other.transform.position);
            Destroy(gameObject);
            return;
        }

        Debug.Log($"{other.name} no es un objetivo válido para el gancho.");
        Destroy(gameObject);
    }
}
