using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [Header("Atributos")]
    public float vidaMaxima = 100f;
    public bool puedeSerAturdido = true;
    public bool puedeSerAtraido = true;
    public bool puedeSerEmpujado = true;

    private float vidaActual;
    private bool estaAturdido = false;
    private Rigidbody rb;
    private IAEnemigo iaScript; // Referencia al script de IA del enemigo

    private void Start()
    {
        vidaActual = vidaMaxima;
        rb = GetComponent<Rigidbody>();
        iaScript = GetComponent<IAEnemigo>();
    }

    // M�todo para recibir da�o
    public void RecibirDano(float cantidad)
    {
        if (estaAturdido) return; // No recibir da�o mientras est� aturdido

        vidaActual -= cantidad;
        Debug.Log($"{gameObject.name} recibi� {cantidad} de da�o. Vida restante: {vidaActual}");

        if (vidaActual <= 0)
        {
            Muerte();
        }
    }

    // M�todo para manejar el aturdimiento
    public void Aturdir(float duracion)
    {
        if (!puedeSerAturdido || estaAturdido) return;

        estaAturdido = true;
        Debug.Log($"{gameObject.name} ha sido aturdido por {duracion} segundos.");

        if (iaScript != null)
        {
            iaScript.enabled = false; // Desactivar IA
        }

        Invoke(nameof(FinAturdimiento), duracion);
    }

    private void FinAturdimiento()
    {
        estaAturdido = false;
        Debug.Log($"{gameObject.name} ya no est� aturdido.");

        if (iaScript != null)
        {
            iaScript.enabled = true; // Reactivar IA
        }
    }

    // M�todo para ser atra�do por el gancho
    public void SerAtraido(Vector3 posicionJugador, float velocidadAtraer)
    {
        if (!puedeSerAtraido) return;

        Debug.Log($"{gameObject.name} est� siendo atra�do hacia el jugador.");
        StartCoroutine(MoverHaciaJugador(posicionJugador, velocidadAtraer));
    }

    private System.Collections.IEnumerator MoverHaciaJugador(Vector3 posicionJugador, float velocidadAtraer)
    {
        while (Vector3.Distance(transform.position, posicionJugador) > 1f)
        {
            Vector3 direccion = (posicionJugador - transform.position).normalized;
            rb.MovePosition(transform.position + direccion * velocidadAtraer * Time.deltaTime);
            yield return null;
        }
        Debug.Log($"{gameObject.name} ha llegado al jugador.");
    }

    // M�todo para ser empujado por la habilidad de aire
    public void SerEmpujado()
    {

        Debug.Log($"{gameObject.name} est� siendo empujado.");
        iaScript.enabled = false;
        Invoke("SI",2);


    }
    public void SI()
    {
        iaScript.enabled = true;
    }

    // M�todo de muerte
    private void Muerte()
    {
        Debug.Log($"{gameObject.name} ha muerto.");
        Destroy(gameObject);
    }
}
