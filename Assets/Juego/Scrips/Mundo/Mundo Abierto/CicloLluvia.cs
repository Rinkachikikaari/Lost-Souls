using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;

public class CicloLluvia : MonoBehaviour
{
    public ParticleSystem lluviaParticulas;   // Sistema de partículas para la lluvia
    public float intervaloLluviaMin = 30f;    // Tiempo mínimo entre lluvias en segundos
    public float intervaloLluviaMax = 120f;   // Tiempo máximo entre lluvias en segundos
    public float duracionLluvia = 20f;        // Duración de la lluvia en segundos
    public float esperaEntreCharcos = 20f;        // Duración de la lluvia en segundos

    public GameObject prefabCharcoLodo; // Asigna el prefab del charco de lodo

    private List<GameObject> charcosInstanciados = new List<GameObject>();

    private bool estaLloviendo = false;
    private GameObject Player_;

    bool lluviaActiva = false;

    private void Start()
    {
 
        StartCoroutine(CicloDeLluvia());
        StartCoroutine(GenerarCharcos());
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
            lluviaActiva = true;

            // Esperar a que termine la lluvia
            yield return new WaitForSeconds(duracionLluvia);

            // Detener la lluvia
            DetenerLluvia();
            lluviaActiva = false;
        }
    }

    private IEnumerator GenerarCharcos()
    {
        while (true)
        {
            if (lluviaActiva)
            {
                yield return new WaitForSeconds(esperaEntreCharcos);
                // LimpiarCharcosDeLodo();
                GenerarCharcosDeLodo();
            }
        }
    }

    private void ComenzarLluvia()
    {
        estaLloviendo = true;
        lluviaParticulas.Play();

        GenerarCharcosDeLodo();
    }

    private void DetenerLluvia()
    {
        estaLloviendo = false;
        lluviaParticulas.Stop();
        LimpiarCharcosDeLodo();
    }

    private void GenerarCharcosDeLodo()
    {
        Player_ = GameObject.FindGameObjectWithTag("Player");
        // Crear algunos charcos en posiciones aleatorias
        for (int i = 0; i < 20; i++) // Por ejemplo, generar 5 charcos
        {
            Vector3 posicionAleatoria = new Vector3(
                Random.Range(Player_.transform.position.x -20f, Player_.transform.position.x + 20f),
                -0.994f,  // Ajusta la altura si es necesario
                Random.Range(Player_.transform.position.z- 20f, Player_.transform.position.z + 20f)
            );

            // Instanciar el charco con una rotación de 90 grados en el eje X
            GameObject charco = Instantiate(prefabCharcoLodo, posicionAleatoria, Quaternion.Euler(90, 0, 0));
            charcosInstanciados.Add(charco);
        }
    }

    private void LimpiarCharcosDeLodo()
    {
        // Eliminar o desactivar todos los charcos generados
        foreach (GameObject charco in charcosInstanciados)
        {
            Destroy(charco); // O charco.SetActive(false);
        }
        charcosInstanciados.Clear();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
