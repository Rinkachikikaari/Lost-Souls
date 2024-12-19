using UnityEngine;

public class PlayerFallHandler : MonoBehaviour
{
    [Header("Configuración")]
    public Collider detectorDeCaida; // Collider específico que detectará las caídas
    public string tagCaida = "Caida"; // Tag para detectar caída
    public string triggerCaida = "caida"; // Trigger en el Animator
    public LayerMask layerSuelo; // Layer para identificar el suelo

    private Animator animator; // Animator del jugador
    private Vector3 ultimaPosicionSegura; // Última posición segura registrada
    private Transform ultimoBloqueSeguro; // Último bloque seguro registrado
    private bool enCaida = false; // Estado de caída

    // Scripts del jugador que se deben desactivar
    private Movimiento movimientoScript;
    private DisparoFlecha arcoScript;
    private Gancho ganchoScript;
    private SelectorDeMagia selectorDeMagia;

    private void Start()
    {
        // Obtener componentes necesarios del jugador
        animator = GetComponentInParent<Animator>();
        movimientoScript = GetComponentInParent<Movimiento>();
        arcoScript = GetComponentInParent<DisparoFlecha>();
        ganchoScript = GetComponentInParent<Gancho>();
        selectorDeMagia = GetComponentInParent<SelectorDeMagia>();

        if (animator == null)
        {
            Debug.LogError("No se encontró un Animator en el jugador.");
        }

        // Inicializar la última posición segura en la posición inicial del jugador
        ultimaPosicionSegura = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar el tag "Caida" en el collider configurado
        if (other.CompareTag(tagCaida) && detectorDeCaida.bounds.Intersects(other.bounds))
        {
            ActivarCaida(other.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Registrar última posición segura y el bloque al tocar un objeto con el Layer Suelo
        if ((layerSuelo.value & (1 << collision.gameObject.layer)) > 0)
        {
            ultimoBloqueSeguro = collision.transform; // Guarda el bloque seguro
            ultimaPosicionSegura = collision.transform.position; // Guarda la posición del bloque
            Debug.Log($"Bloque seguro registrado: {ultimoBloqueSeguro.name} en posición {ultimaPosicionSegura}");
        }
    }

    private void ActivarCaida(Vector3 posicionCaida)
    {
        if (enCaida) return; // Evitar múltiples activaciones

        enCaida = true;

        // Desactivar scripts de control
        movimientoScript.enabled = false;
        arcoScript.enabled = false;
        ganchoScript.enabled = false;
        selectorDeMagia.enabled = false;

        // Activar la animación de caída
        if (animator != null)
        {
            animator.SetTrigger(triggerCaida);
        }

        // Bloquear al jugador en la posición de caída visualmente
        transform.position = new Vector3(posicionCaida.x, transform.position.y, posicionCaida.z);

        Debug.Log("Caída detectada. Activando animación...");
    }

    /// <summary>
    /// Método llamado al finalizar la animación de caída.
    /// </summary>
    public void ReposicionarJugador()
    {
        if (!enCaida) return;

        enCaida = false;

        float Y = 1.185079f;

        if (ultimoBloqueSeguro != null)
        {
            // Reposicionar al centro del último bloque seguro
            Vector3 posicionReposicion = new Vector3(
                ultimoBloqueSeguro.position.x,
                Y, // Ajuste en Y para colocarlo sobre el bloque
                ultimoBloqueSeguro.position.z
            );
            transform.position = posicionReposicion;

            Debug.Log($"Jugador reposicionado al bloque seguro: {ultimoBloqueSeguro.name} en posición {posicionReposicion}");
        }
        else
        {
            Debug.LogWarning("No se encontró un bloque seguro. Manteniendo la última posición registrada.");
            transform.position = ultimaPosicionSegura;
        }
        VidaJugador.instance.RecibirDano(1);

        // Reactivar scripts de control
        movimientoScript.enabled = true;
        arcoScript.enabled = true;
        ganchoScript.enabled = true;
        selectorDeMagia.enabled = true;
    }
}
