using System.Collections;
using UnityEngine;

public class Gancho : MonoBehaviour
{
    public GameObject prefabGancho;           // Prefab del objeto gancho
    public Transform puntoDisparo;            // Punto desde el cual se dispara el gancho
    public float ganchoVelocidad = 15f;

    private Rigidbody rb;
    private Movimiento movimientoScript;      // Referencia al script de movimiento
    private Ataque ataqueScript;              // Referencia al script de ataque
    private bool ganchoActivo = false;
    private Vector3 ultimaDireccion;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movimientoScript = GetComponent<Movimiento>();
        ataqueScript = GetComponent<Ataque>(); // Obtener el script de ataque
    }

    private void Update()
    {
        // Guardar la última dirección de movimiento si el jugador se está moviendo
        if (movimientoScript.movement != Vector3.zero)
        {
            ultimaDireccion = movimientoScript.movement;
        }

        // Lanzar el gancho en la última dirección de movimiento si el jugador presiona la tecla G
        if (Input.GetKeyDown(KeyCode.G) && !ganchoActivo)
        {
            LanzarGancho();
        }
    }

    private void LanzarGancho()
    {
        // Asegurarse de que el prefab esté asignado y de que exista una dirección válida
        if (prefabGancho != null && ultimaDireccion != Vector3.zero)
        {
            // Instanciar el objeto gancho en el punto de disparo
            GameObject ganchoInstancia = Instantiate(prefabGancho, puntoDisparo.position, Quaternion.identity);
            if (ganchoInstancia != null)
            {
                // Configurar la dirección y los valores del objeto gancho
                GanchoObjeto ganchoScript = ganchoInstancia.GetComponent<GanchoObjeto>();
                ganchoScript.ConfigurarGancho(ultimaDireccion, transform, this);

                // Bloquear el movimiento y ataque del jugador
                movimientoScript.enabled = false;
                ataqueScript.enabled = false; // Desactivar ataque
                ganchoActivo = true;
            }
            else
            {
                Debug.LogError("El gancho no se pudo instanciar.");
            }
        }
        else
        {
            Debug.LogWarning("Prefab de gancho no asignado o no hay dirección de movimiento.");
        }
    }

    // Método llamado por GanchoObjeto para atraer el objetivo hacia el jugador
    public void AtraerObjetivo(Transform objetivo)
    {
        StartCoroutine(AtraerObjetoHaciaJugador(objetivo));
    }

    private IEnumerator AtraerObjetoHaciaJugador(Transform objetivo)
    {
        while (Vector3.Distance(objetivo.position, transform.position) > 1f)
        {
            objetivo.position = Vector3.MoveTowards(objetivo.position, transform.position, ganchoVelocidad * Time.deltaTime);
            yield return null;
        }
        FinGancho();
    }

    // Método llamado por GanchoObjeto para mover al jugador hacia la superficie
    public void MoverHaciaSuperficie(Vector3 posicionSuperficie)
    {
        StartCoroutine(MoverJugadorHaciaSuperficie(posicionSuperficie));
    }

    private IEnumerator MoverJugadorHaciaSuperficie(Vector3 posicionObjetivo)
    {
        while (Vector3.Distance(transform.position, posicionObjetivo) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, ganchoVelocidad * Time.deltaTime);
            yield return null;
        }
        FinGancho();
    }

    // Método para reactivar el movimiento y ataque del jugador y reiniciar el gancho
    public void FinGancho()
    {
        ganchoActivo = false;
        movimientoScript.enabled = true; // Reactivar movimiento
        ataqueScript.enabled = true;     // Reactivar ataque
    }
}
