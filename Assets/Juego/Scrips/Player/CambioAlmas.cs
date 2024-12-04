using UnityEngine;

public class CambioAlmas : MonoBehaviour
{
    [Header("Configuración de las Almas")]
    public GameObject almaActual; // Representa al alma actual (cuerpo del personaje)
    public GameObject almaPasada; // Representa al alma pasada
    public KeyCode teclaCambio = KeyCode.F; // Tecla para cambiar entre almas
    public KeyCode teclaActivar = KeyCode.E; // Tecla para activar el cambio cerca del fuego

    [Header("Distancia Máxima")]
    public float distanciaMaxima = 5f; // Distancia máxima permitida entre las almas

    [Header("Cámara")]
    public CamarasAlmas camara; // Referencia al script de la cámara

    [Header("Componentes")]
    public Movimiento movimientoScript;
    public DisparoFlecha ArcoScript;
    public Gancho GanchoScript;
    public SelectorDeMagia SelectorDeMagia;

    [SerializeField] string HerramientaActiva;

    private bool cercaDelFuego = false;
    private bool cambioPermitido = false;
    private bool controlAlmaActual = true;

    private void Start()
    {
        ActivarAlmaActual();
    }

    private void Update()
    {
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

        movimientoScript.enabled = true;
        ArcoScript.enabled = true;
        GanchoScript.enabled = true;
        SelectorDeMagia.enabled = true;

        almaPasada.SetActive(false);
        camara.CambiarCamara(true); // Cambia la cámara al alma actual
    }

    private void ActivarAlmaPasada()
    {
        controlAlmaActual = false;

        movimientoScript.enabled = false;
        ArcoScript.enabled = false;
        GanchoScript.enabled = false;
        SelectorDeMagia.enabled = false;

        almaPasada.SetActive(true);
        camara.CambiarCamara(false); // Cambia la cámara al alma pasada
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
