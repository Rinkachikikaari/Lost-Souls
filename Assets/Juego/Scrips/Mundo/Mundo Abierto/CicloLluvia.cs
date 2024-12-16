using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CicloLluvia : MonoBehaviour
{
    public ParticleSystem lluviaParticulas;   // Sistema de part�culas para la lluvia
    public float intervaloLluviaMin = 30f;    // Tiempo m�nimo entre lluvias
    public float intervaloLluviaMax = 120f;   // Tiempo m�ximo entre lluvias
    public float duracionLluvia = 20f;        // Duraci�n de la lluvia
    public float esperaEntreCharcos = 5f;     // Tiempo entre generaci�n de charcos

    public GameObject prefabCharcoLodo;       // Prefab de charcos
    public int maxCharcosPorLluvia = 20;      // M�ximo de charcos por lluvia

    private bool estaLloviendo = false;
    private bool jugadorEnZonaCubierta = false; // Estado del jugador respecto a las zonas cubiertas
    private GameObject player;

    private Coroutine corutinaGenerarCharcos;

    private void Start()
    {
        // Encontrar al jugador al inicio
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No se encontr� un objeto con la etiqueta 'Player'.");
            return;
        }

        // Iniciar el ciclo de lluvia
        StartCoroutine(CicloDeLluvia());
    }

    private IEnumerator CicloDeLluvia()
    {
        while (true)
        {
            // Esperar un intervalo aleatorio antes de comenzar la lluvia
            float tiempoEspera = Random.Range(intervaloLluviaMin, intervaloLluviaMax);
            yield return new WaitForSeconds(tiempoEspera);

            // Iniciar la lluvia
            ComenzarLluvia();

            // Esperar la duraci�n de la lluvia
            yield return new WaitForSeconds(duracionLluvia);

            // Detener la lluvia
            DetenerLluvia();
        }
    }

    private void ComenzarLluvia()
    {
        if (!estaLloviendo)
        {
            estaLloviendo = true;

            // Solo mostrar part�culas si el jugador no est� en una zona cubierta
            if (!jugadorEnZonaCubierta)
            {
                lluviaParticulas.Play();
            }

            // Iniciar la corutina para generar charcos
            if (corutinaGenerarCharcos == null)
            {
                corutinaGenerarCharcos = StartCoroutine(GenerarCharcos());
            }
        }
    }

    private void DetenerLluvia()
    {
        if (estaLloviendo)
        {
            estaLloviendo = false;
            lluviaParticulas.Stop();

            // Detener la corutina de generaci�n de charcos
            if (corutinaGenerarCharcos != null)
            {
                StopCoroutine(corutinaGenerarCharcos);
                corutinaGenerarCharcos = null;
            }

            // Limpia los charcos generados (si es necesario)
        }
    }

    private IEnumerator GenerarCharcos()
    {
        int charcosGenerados = 0;

        while (estaLloviendo && charcosGenerados < maxCharcosPorLluvia)
        {
            // Esperar antes de intentar generar el siguiente charco
            yield return new WaitForSeconds(esperaEntreCharcos);

            // Si el jugador est� en una zona cubierta, no generamos charcos
            if (jugadorEnZonaCubierta) continue;

            // Generar un charco cerca del jugador
            Vector3 posicionAleatoria = new Vector3(
                Random.Range(player.transform.position.x - 20f, player.transform.position.x + 20f),
                -0.994f, // Altura del charco
                Random.Range(player.transform.position.z - 20f, player.transform.position.z + 20f)
            );

            Instantiate(prefabCharcoLodo, posicionAleatoria, Quaternion.Euler(90, 0, 0));
            charcosGenerados++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar si el jugador entra en una zona cubierta
        if (other.CompareTag("ZonaCubierta"))
        {
            jugadorEnZonaCubierta = true;

            // Detener las part�culas de lluvia, pero no el contador ni los charcos
            if (lluviaParticulas.isPlaying)
            {
                lluviaParticulas.Stop();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Detectar si el jugador sale de una zona cubierta
        if (other.CompareTag("ZonaCubierta"))
        {
            jugadorEnZonaCubierta = false;

            // Reanudar las part�culas si est� lloviendo
            if (estaLloviendo && !lluviaParticulas.isPlaying)
            {
                lluviaParticulas.Play();
            }
        }
    }
}
