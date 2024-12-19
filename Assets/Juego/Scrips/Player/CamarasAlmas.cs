using UnityEngine;
using Cinemachine;

public class CamarasAlmas : MonoBehaviour
{
    [Header("C�maras Virtuales")]
    public CinemachineVirtualCamera camaraActual; // VCam para el alma actual
    public CinemachineVirtualCamera camaraPasada; // VCam para el alma pasada

    [Header("Prioridades")]
    public int prioridadAlta = 10; // Prioridad de la c�mara activa
    public int prioridadBaja = 5; // Prioridad de la c�mara inactiva

    private Camera mainCamera; // Referencia a la c�mara principal

    private void Start()
    {
        // Obtenemos la c�mara principal (Main Camera)
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No se encontr� una c�mara principal (Main Camera).");
        }
    }

    /// <summary>
    /// Cambia el objetivo de la c�mara seg�n el alma activa.
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
