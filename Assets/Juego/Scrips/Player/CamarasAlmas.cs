using Cinemachine;
using UnityEngine;

public class CamarasAlmas : MonoBehaviour
{
    [Header("C�maras Virtuales")]
    public CinemachineVirtualCamera camaraActual; // VCam para el alma actual
    public CinemachineVirtualCamera camaraPasada; // VCam para el alma pasada

    [Header("Prioridades")]
    public int prioridadAlta = 10; // Prioridad de la c�mara activa
    public int prioridadBaja = 5; // Prioridad de la c�mara inactiva

    /// <summary>
    /// Cambia el objetivo de la c�mara seg�n el alma activa.
    /// </summary>
    /// <param name="esAlmaActual">Si true, sigue al alma actual; si false, al alma pasada.</param>
    public void CambiarCamara(bool esAlmaActual)
    {
        if (esAlmaActual)
        {
            camaraActual.Priority = prioridadAlta;
            camaraPasada.Priority = prioridadBaja;
            Debug.Log("C�mara cambiada al alma actual.");
        }
        else
        {
            camaraActual.Priority = prioridadBaja;
            camaraPasada.Priority = prioridadAlta;
            Debug.Log("C�mara cambiada al alma pasada.");
        }
    }
}
