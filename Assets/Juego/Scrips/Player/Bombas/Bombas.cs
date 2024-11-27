using UnityEngine;

public class Bombas : MonoBehaviour
{
    [Header("Configuraci�n de la Bomba")]
    public float tiempoExplosion = 3f; // Tiempo antes de que explote
    public float radioExplosion = 5f; // Radio de la explosi�n
    public int da�o = 4; // Da�o que causa la bomba
    public LayerMask capasAfectadas; // Capas que ser�n afectadas por la explosi�n

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
        Debug.Log("�BOOM! La bomba ha explotado.");
        // Detectar objetos en el radio de explosi�n
        Collider[] objetosAfectados = Physics.OverlapSphere(transform.position, radioExplosion, capasAfectadas);

        foreach (var objeto in objetosAfectados)
        {
            // Aplicar da�o a enemigos o jugador
            VidaJugador vida = objeto.GetComponent<VidaJugador>();
            if (vida != null)
            {
                vida.RecibirDano(da�o);
            }

            Enemy enemy = objeto.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.RecibirDano(da�o);
            }

            // Destruir estructuras
            EstructuraDestruible estructura = objeto.GetComponent<EstructuraDestruible>();
            if (estructura != null)
            {
                estructura.Destruir();
            }
        }

        // Notificar al jugador que la bomba explot�
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

    // Dibujar el radio de explosi�n como un gizmo visible en el editor
    private void OnDrawGizmosSelected()
    {
        // Configura el color del gizmo
        Gizmos.color = new Color(1, 0, 0, 0.5f); // Rojo semitransparente
        // Dibuja una esfera que represente el radio de la explosi�n
        Gizmos.DrawSphere(transform.position, radioExplosion);

        // Dibuja el contorno del radio para mayor claridad
        Gizmos.color = Color.red; // Rojo s�lido
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}
