using UnityEngine;

public class Bombas : MonoBehaviour
{
    [Header("Configuración de la Bomba")]
    public float tiempoExplosion = 3f; // Tiempo antes de que explote
    public float radioExplosion = 5f; // Radio de la explosión
    public int daño = 4; // Daño que causa la bomba
    public LayerMask capasAfectadas; // Capas que serán afectadas por la explosión

    public delegate void BombaExplotada(Bombas bomba);
    public event BombaExplotada OnBombaExplotada;

    private bool lanzada = false; // Si la bomba ha sido lanzada
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Iniciar cuenta regresiva
        Invoke(nameof(Explotar), tiempoExplosion);
    }

    private void Explotar()
    {
        Debug.Log("¡BOOM! La bomba ha explotado.");
        // Detectar objetos en el radio de explosión
        Collider[] objetosAfectados = Physics.OverlapSphere(transform.position, radioExplosion, capasAfectadas);

        foreach (var objeto in objetosAfectados)
        {
            // Aplicar daño a enemigos o jugador
            VidaJugador vida = objeto.GetComponent<VidaJugador>();
            if (vida != null)
            {
                vida.RecibirDano(daño);
            }

            Enemy enemy = objeto.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.RecibirDano(daño);
            }

            // Destruir estructuras
            EstructuraDestruible estructura = objeto.GetComponent<EstructuraDestruible>();
            if (estructura != null)
            {
                estructura.Destruir();
            }
        }

        // Notificar al jugador que la bomba explotó
        OnBombaExplotada?.Invoke(this);

        // Destruir la bomba tras explotar
        Destroy(gameObject);
    }

    public void Lanzar(Vector3 direccion, float fuerza)
    {
        if (!lanzada && rb != null)
        {
            rb.AddForce(direccion * fuerza, ForceMode.Impulse);
            lanzada = true;
        }
    }

    // Dibujar el radio de explosión como un gizmo visible en el editor
    private void OnDrawGizmosSelected()
    {
        // Configura el color del gizmo
        Gizmos.color = new Color(1, 0, 0, 0.5f); // Rojo semitransparente
        // Dibuja una esfera que represente el radio de la explosión
        Gizmos.DrawSphere(transform.position, radioExplosion);

        // Dibuja el contorno del radio para mayor claridad
        Gizmos.color = Color.red; // Rojo sólido
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}
