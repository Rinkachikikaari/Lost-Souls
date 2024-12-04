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

    // Método para recibir daño
    public void RecibirDano(float cantidad)
    {
        if (estaAturdido) return; // No recibir daño mientras está aturdido

        vidaActual -= cantidad;
        Debug.Log($"{gameObject.name} recibió {cantidad} de daño. Vida restante: {vidaActual}");

        if (vidaActual <= 0)
        {
            Muerte();
        }
    }

    // Método para manejar el aturdimiento
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
        Debug.Log($"{gameObject.name} ya no está aturdido.");

        if (iaScript != null)
        {
            iaScript.enabled = true; // Reactivar IA
        }
    }

    // Método para ser atraído por el gancho
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
            rb.MovePosition(transform.position + direccion * velocidadAtraer * Time.deltaTime);
            yield return null;
        }
        Debug.Log($"{gameObject.name} ha llegado al jugador.");
    }

    // Método para ser empujado por la habilidad de aire
    public void SerEmpujado()
    {

        Debug.Log($"{gameObject.name} está siendo empujado.");
        iaScript.enabled = false;
        Invoke("SI",2);


    }
    public void SI()
    {
        iaScript.enabled = true;
    }

    // Método de muerte
    private void Muerte()
    {
        Debug.Log($"{gameObject.name} ha muerto.");
        Destroy(gameObject);
    }
}
