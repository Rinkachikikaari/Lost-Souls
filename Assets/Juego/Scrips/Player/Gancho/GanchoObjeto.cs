using UnityEngine;

public class GanchoObjeto : MonoBehaviour
{
    public float velocidadGancho = 15f;          // Velocidad del gancho
    public float distanciaMaxima = 10f;          // Distancia m�xima que el gancho puede alcanzar
    private Vector3 direccionGancho;
    private Transform jugador;
    private Gancho ganchoScript;

    private Vector3 puntoInicio;

    private void Start()
    {
        puntoInicio = transform.position;
    }

    public void ConfigurarGancho(Vector3 direccion, Transform jugadorTransform, Gancho gancho)
    {
        direccionGancho = direccion.normalized;
        jugador = jugadorTransform;
        ganchoScript = gancho;
    }

    private void Update()
    {
        // Mover el gancho en la direcci�n especificada
        transform.position += direccionGancho * velocidadGancho * Time.deltaTime;

        // Verificar si se ha alcanzado la distancia m�xima
        if (Vector3.Distance(transform.position, puntoInicio) >= distanciaMaxima)
        {
            RetornarGancho();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo") || other.CompareTag("Objeto"))
        {
            // Atraer el enemigo o el objeto hacia el jugador
            ganchoScript.AtraerObjetivo(other.transform);
        }
        else if (other.CompareTag("Superficie"))
        {
            // Mover al jugador hacia la superficie
            ganchoScript.MoverHaciaSuperficie(other.transform.position);
        }

        // Destruir el gancho despu�s de la colisi�n
        Destroy(gameObject);
    }

    private void RetornarGancho()
    {
        // Destruir el gancho si no ha colisionado y alcanza la distancia m�xima
        Destroy(gameObject);
        ganchoScript.FinGancho();
    }
}
