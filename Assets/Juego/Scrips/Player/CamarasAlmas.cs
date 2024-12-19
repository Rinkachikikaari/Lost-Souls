using UnityEngine;
using Cinemachine;

public class CamarasAlmas : MonoBehaviour
{
    [Header("Cámaras Virtuales")]
    public CinemachineVirtualCamera camaraActual; // VCam para el alma actual
    public CinemachineVirtualCamera camaraPasada; // VCam para el alma pasada

    [Header("Prioridades")]
    public int prioridadAlta = 10; // Prioridad de la cámara activa
    public int prioridadBaja = 5; // Prioridad de la cámara inactiva

    private Camera mainCamera; // Referencia a la cámara principal

    private void Start()
    {
        // Obtenemos la cámara principal (Main Camera)
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No se encontró una cámara principal (Main Camera).");
        }
    }

    /// <summary>
    /// Cambia el objetivo de la cámara según el alma activa.
    /// </summary>
    /// <param name="esAlmaActual">Si true, sigue al alma actual; si false, al alma pasada.</param>
    public void CambiarCamara(bool esAlmaActual)
    {
        if (mainCamera == null) return;

        if (esAlmaActual)
        {
            camaraActual.Priority = prioridadAlta;
            camaraPasada.Priority = prioridadBaja;
        }
        else
        {
            camaraActual.Priority = prioridadBaja;
            camaraPasada.Priority = prioridadAlta;
        }
    }
}
