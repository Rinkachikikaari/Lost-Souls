using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CicloLluvia : MonoBehaviour
{
    public ParticleSystem lluviaParticulas;   // Sistema de partículas para la lluvia
    public float intervaloLluviaMin = 30f;    // Tiempo mínimo entre lluvias en segundos
    public float intervaloLluviaMax = 120f;   // Tiempo máximo entre lluvias en segundos
    public float duracionLluvia = 20f;        // Duración de la lluvia en segundos

    public GameObject prefabCharcoLodo; // Asigna el prefab del charco de lodo

    private List<GameObject> charcosInstanciados = new List<GameObject>();

    private bool estaLloviendo = false;

    private void Start()
    {
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

            // Esperar a que termine la lluvia
            yield return new WaitForSeconds(duracionLluvia);

            // Detener la lluvia
            DetenerLluvia();
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
        // Crear algunos charcos en posiciones aleatorias
        for (int i = 0; i < 10; i++) // Por ejemplo, generar 5 charcos
        {
            Vector3 posicionAleatoria = new Vector3(
                Random.Range(-10f, 10f),
                -0.994f,  // Ajusta la altura si es necesario
                Random.Range(-10f, 10f)
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
}
