using UnityEngine;

public class IAEnemigo : MonoBehaviour
{
    public Transform[] puntosPatrulla;
    public float distanciaVision = 10f;
    public float distanciaAtaque = 2f;
    public float velocidadPatrulla = 2f;
    public float velocidadPersecucion = 4f;
    public float cooldownAtaque = 3f;
    public float distanciaCelda = 1f;

    private Transform jugador;
    private int indicePatrulla = 0;
    private bool persiguiendo = false;
    private Vector3 direccionMovimiento;
    private Vector3 destinoActual;
    private Animator anim;

    private float tiempoUltimoAtaque = -Mathf.Infinity;
    private Vector3 puntoInicioCelda;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        SiguientePuntoPatrulla();
    }

    private void Update()
    {
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= distanciaAtaque)
        {
            if (Time.time >= tiempoUltimoAtaque + cooldownAtaque)
            {
                AtacarJugador();
                tiempoUltimoAtaque = Time.time;
            }
        }
        else if (distanciaAlJugador <= distanciaVision)
        {
            // Si el jugador está en el rango de visión, retoma la persecución
            PerseguirJugador();
        }
        else
        {
            // Si el jugador está fuera del rango de visión, vuelve a patrullar
            if (persiguiendo)
            {
                persiguiendo = false;
                RegresarAPatrulla();
            }
            else
            {
                Patrullar();
            }
        }

        MoverEn4Direcciones();
        ActualizarAnimacionMovimiento();
    }

    private void Patrullar()
    {
        if (Vector3.Distance(transform.position, destinoActual) < 0.5f)
        {
            SiguientePuntoPatrulla();
        }
        else
        {
            if (Vector3.Distance(transform.position, puntoInicioCelda) >= distanciaCelda)
            {
                CalcularDireccionMovimiento(destinoActual);
                puntoInicioCelda = transform.position;
            }
        }
    }

    private void SiguientePuntoPatrulla()
    {
        if (puntosPatrulla.Length == 0) return;

        destinoActual = puntosPatrulla[indicePatrulla].position;
        indicePatrulla = (indicePatrulla + 1) % puntosPatrulla.Length;
        CalcularDireccionMovimiento(destinoActual);
        puntoInicioCelda = transform.position;
    }

    private void PerseguirJugador()
    {
        persiguiendo = true;
        destinoActual = jugador.position;

        if (Vector3.Distance(transform.position, puntoInicioCelda) >= distanciaCelda)
        {
            CalcularDireccionMovimiento(destinoActual);
            puntoInicioCelda = transform.position;
        }
    }

    private void RegresarAPatrulla()
    {
        destinoActual = puntosPatrulla[indicePatrulla].position;
        CalcularDireccionMovimiento(destinoActual);
    }

    private void AtacarJugador()
    {
        // El enemigo ataca y luego vuelve a perseguir si el jugador está en rango de visión
        persiguiendo = true;
        direccionMovimiento = Vector3.zero;
        anim.SetTrigger("Attack");
        Debug.Log("Atacando al jugador!");

    }

    private void CalcularDireccionMovimiento(Vector3 destino)
    {
        Vector3 direccion = destino - transform.position;

        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
        {
            direccionMovimiento = new Vector3(Mathf.Sign(direccion.x), 0, 0);
        }
        else
        {
            direccionMovimiento = new Vector3(0, 0, Mathf.Sign(direccion.z));
        }
    }

    private void MoverEn4Direcciones()
    {
        transform.position += direccionMovimiento * (persiguiendo ? velocidadPersecucion : velocidadPatrulla) * Time.deltaTime;
    }

    private void ActualizarAnimacionMovimiento()
    {
        anim.SetFloat("MoveX", direccionMovimiento.x);
        anim.SetFloat("MoveZ", direccionMovimiento.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaVision);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
    }
}
