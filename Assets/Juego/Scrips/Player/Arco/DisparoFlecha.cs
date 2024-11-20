using UnityEngine;

public class DisparoFlecha : MonoBehaviour
{
    public GameObject flechaPrefab;
    public Transform puntoDisparo;
    public float velocidadMin = 5f;
    public float velocidadMax = 20f;
    public float tiempoCargaMax = 2f;

    public float estaminaPorDisparo = 10f;         // Estamina consumida en disparo normal
    public float estaminaPorDisparoMax = 20f;      // Estamina consumida en disparo al máximo

    private float tiempoCargando = 0f;
    private bool cargandoDisparo = false;    
    private Vector3 ultimaDireccion;

    private Movimiento movimientoScript;
    private EstaminaJugador estaminaJugador;       // Referencia al script de estamina
    private Ataque AtaqueScript; 
    private Gancho GanchosScript;
    private SelectorDeMagia SelectorDeMagia;

    void Start()
    {
        movimientoScript = GetComponent<Movimiento>();
        estaminaJugador = GetComponent<EstaminaJugador>();
        GanchosScript = GetComponent<Gancho>();
        AtaqueScript = GetComponent<Ataque>();
        SelectorDeMagia = GetComponent<SelectorDeMagia>();
    }

    void Update()
    {
        if (movimientoScript.movement != Vector3.zero)
        {
            ultimaDireccion = movimientoScript.movement;
        }

        if (Input.GetKey(KeyCode.L) && InventoryManager.instance.HasItem("Arco"))
        {
            cargandoDisparo = true;
            tiempoCargando += Time.deltaTime;
            movimientoScript.enabled = false;
            AtaqueScript.enabled = false;
            GanchosScript.enabled = false;
            SelectorDeMagia.enabled = false;
            tiempoCargando = Mathf.Clamp(tiempoCargando, 0, tiempoCargaMax);
        }

        if (Input.GetKeyUp(KeyCode.L) && cargandoDisparo && InventoryManager.instance.HasItem("Arco"))
        {
            float estaminaNecesaria = tiempoCargando >= tiempoCargaMax ? estaminaPorDisparoMax : estaminaPorDisparo;

            // Verificar si hay suficiente estamina antes de disparar
            if (estaminaJugador.UsarEstamina(estaminaNecesaria))
            {
                DispararFlecha();
            }
            else
            {
                Debug.Log("No hay suficiente estamina para disparar.");
            }

            cargandoDisparo = false;
            tiempoCargando = 0;
            movimientoScript.enabled = true;
            AtaqueScript.enabled=true;
            GanchosScript.enabled=true;
            SelectorDeMagia.enabled=true;
        }
    }

    void DispararFlecha()
    {
        if (ultimaDireccion != Vector3.zero)
        {
            GameObject nuevaFlecha = Instantiate(flechaPrefab, puntoDisparo.position, Quaternion.LookRotation(ultimaDireccion));
            float velocidad = Mathf.Lerp(velocidadMin, velocidadMax, tiempoCargando / tiempoCargaMax);
            Rigidbody rbFlecha = nuevaFlecha.GetComponent<Rigidbody>();
            rbFlecha.linearVelocity = ultimaDireccion * velocidad;
        }
        else
        {
            Debug.LogWarning("No hay dirección de movimiento para disparar la flecha.");
        }
    }
}
