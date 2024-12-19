using UnityEngine;
using System;

public class CambioAlmas : MonoBehaviour
{
    public static event Action<bool> OnCambioAlma; // Evento que notifica el cambio de alma

    [Header("Configuración de las Almas")]
    public GameObject almaActual;
    public GameObject almaPasada;
    public KeyCode teclaCambio = KeyCode.F;
    public KeyCode teclaActivar = KeyCode.E;

    [Header("Distancia Máxima")]
    public float distanciaMaxima = 5f;

    [Header("Cámara")]
    public CamarasAlmas camara;

    [Header("Componentes")]
    public GameObject Lampara;
    public Movimiento movimientoScript;
    public DisparoFlecha ArcoScript;
    public Gancho GanchoScript;
    public SelectorDeMagia SelectorDeMagia;

    [SerializeField] string HerramientaActiva;

    private bool cercaDelFuego = false;
    private bool cambioPermitido = false;
    public static bool controlAlmaActual = true;

    private void Start()
    {
        ActivarAlmaActual();
    }

    private void Update()
    {
        if (HerramientaActiva == "Lampara De Almas")
        {
            Lampara.SetActive(true);
        }
        else
        {
            Lampara.SetActive(false);
        }
        if (cercaDelFuego && Input.GetKeyDown(teclaActivar) && HerramientaActiva == "Lampara De Almas")
        {
            cambioPermitido = true;
        }

        if (cambioPermitido && Input.GetKeyDown(teclaCambio) && HerramientaActiva == "Lampara De Almas")
        {
            if (controlAlmaActual)
            {
                ActivarAlmaPasada();
            }
            else
            {
                ActivarAlmaActual();
            }
        }

        if (!controlAlmaActual && Vector3.Distance(almaActual.transform.position, almaPasada.transform.position) > distanciaMaxima)
        {
            ReunirAlmaPasada();
        }
    }

    private void ActivarAlmaActual()
    {
        controlAlmaActual = true;
        OnCambioAlma?.Invoke(controlAlmaActual); // Notifica el cambio

        movimientoScript.enabled = true;
        ArcoScript.enabled = true;
        GanchoScript.enabled = true;
        SelectorDeMagia.enabled = true;

        almaPasada.SetActive(false);
        camara.CambiarCamara(true);
    }

    private void ActivarAlmaPasada()
    {
        controlAlmaActual = false;
        OnCambioAlma?.Invoke(controlAlmaActual); // Notifica el cambio


        movimientoScript.enabled = false;
        ArcoScript.enabled = false;
        GanchoScript.enabled = false;
        SelectorDeMagia.enabled = false;


        almaPasada.SetActive(true);
        camara.CambiarCamara(false);
    }

    private void ReunirAlmaPasada()
    {
        almaPasada.transform.position = almaActual.transform.position;
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FuegoAlmas"))
        {
            cercaDelFuego = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FuegoAlmas"))
        {
            cercaDelFuego = false;
            cambioPermitido = false;
        }
    }

    public void UpdateCurrentWeapon()
    {
        if (PlayerStats.instance.HerramientaActiva != null)
        {
            HerramientaActiva = PlayerStats.instance.HerramientaActiva.itemName;
        }
        else
        {
            HerramientaActiva = "Sin Herramienta equipada";
        }

        Debug.Log($"Herramienta actualizada: {HerramientaActiva}");
    }
}
