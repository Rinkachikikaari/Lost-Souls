using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    public Light luzDireccional;  // Luz direccional que actúa como el sol
    public float duracionDia = 120f; // Duración total de un día en segundos (día completo)

    [Range(0, 1)]
    public static float tiempoActual = 0f; // Progreso actual del día (0 = medianoche, 0.5 = mediodía, 1 = medianoche)

    private float velocidadCiclo;

    private void Start()
    {
        velocidadCiclo = 1f / duracionDia; // Calcula la velocidad de avance del tiempo en el ciclo
    }

    private void Update()
    {
        // Avanzar el tiempo
        tiempoActual += velocidadCiclo * Time.deltaTime;
        tiempoActual %= 1; // Asegurarse de que el tiempo esté entre 0 y 1

        // Ajustar la rotación de la luz direccional
        ActualizarLuzDireccional();
        ActualizarColorLuz();
    }

    private void ActualizarLuzDireccional()
    {
        // Rotar la luz direccional para simular el sol
        float anguloRotacion = tiempoActual * 360f - 90f; // -90 para que comience en el amanecer
        luzDireccional.transform.rotation = Quaternion.Euler(new Vector3(anguloRotacion, 170, 0));
    }

    private void ActualizarColorLuz()
    {
        // Cambiar el color de la luz para simular distintos momentos del día
        if (tiempoActual <= 0.25f || tiempoActual >= 0.75f) // Noche
        {
            luzDireccional.intensity = 0.1f;
            RenderSettings.ambientLight = Color.black;
        }
        else if (tiempoActual <= 0.3f) // Amanecer
        {
            float t = Mathf.InverseLerp(0.25f, 0.3f, tiempoActual);
            luzDireccional.intensity = Mathf.Lerp(0.1f, 1f, t);
            RenderSettings.ambientLight = Color.Lerp(Color.black, Color.gray, t);
        }
        else if (tiempoActual <= 0.7f) // Día
        {
            luzDireccional.intensity = 1f;
            RenderSettings.ambientLight = Color.gray;
        }
        else if (tiempoActual <= 0.75f) // Atardecer
        {
            float t = Mathf.InverseLerp(0.7f, 0.75f, tiempoActual);
            luzDireccional.intensity = Mathf.Lerp(1f, 0.1f, t);
            RenderSettings.ambientLight = Color.Lerp(Color.gray, Color.black, t);
        }
    }
}
