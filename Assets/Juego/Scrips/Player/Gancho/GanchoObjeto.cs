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
        // Mover el gancho hacia adelante
        transform.position += direccion * velocidadGancho * Time.deltaTime;

        // Verificar si alcanz� la distancia m�xima
        if (Vector3.Distance(transform.position, jugador.position) > distanciaMaxima)
        {
            Debug.Log("Gancho alcanz� su distancia m�xima.");
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
        // Verificar si es un enemigo
        Enemy enemigo = other.GetComponent<Enemy>();
        if (enemigo != null)
        {
            if (enemigo.puedeSerAtraido)
            {
                ganchoScript.AtraerObjetivo(other.transform);
            }
            else
            {
                Debug.Log($"{other.name} es un enemigo, pero no puede ser atra�do.");
            }
            ganchoScript.FinGancho();
            Destroy(gameObject);
            return;
        }

        // Verificar si es un objeto interactivo
        GanchoInteractivo objetoInteractivo = other.GetComponent<GanchoInteractivo>();
        if (objetoInteractivo != null)
        {
            if (objetoInteractivo.puedeSerAtraido)
            {
                ganchoScript.AtraerObjetivo(other.transform);
            }
            else
            {
                Debug.Log($"{other.name} es un objeto interactivo, pero no puede ser atra�do.");
            }
            ganchoScript.FinGancho();
            Destroy(gameObject);
            return;
        }

        // Verificar si es una superficie v�lida
        if (EsSuperficieValida(other.gameObject))
        {
            ganchoScript.MoverHaciaSuperficie(other.transform.position);
            ganchoScript.FinGancho();
            Destroy(gameObject);
            return;
        }

        // Si no es un objetivo v�lido
        Debug.Log($"{other.name} no es un objetivo v�lido para el gancho.");
        ganchoScript.FinGancho();
        Destroy(gameObject);
    }

    private bool EsSuperficieValida(GameObject superficie)
    {
        return superficie.CompareTag("Superficie") || superficie.layer == LayerMask.NameToLayer("Superficie");
    }

}
