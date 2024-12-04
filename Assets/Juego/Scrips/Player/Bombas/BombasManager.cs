using UnityEngine;

public class BombasManager : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject prefabBomba; // Prefab de la bomba
    public Transform posicionSostener; // Posición sobre el jugador para sostener la bomba
    public float fuerzaLanzamiento = 10f; // Fuerza con la que se lanza la bomba
    public float radioDeteccion = 1.5f; // Radio para detectar bombas cercanas
    public KeyCode teclaInteractuar = KeyCode.E; // Tecla para interactuar con la bomba
    public KeyCode teclaColocar = KeyCode.Q; // Tecla para instanciar la bomba

    [SerializeField] string HerramientaActiva;


    private Bombas bombaActual; // Referencia a la bomba actual
    private Vector3 ultimaDireccion; // Última dirección de movimiento
    private bool sosteniendoBomba = false;

    private void Update()
    {
        // Capturar la última dirección de movimiento
        CapturarDireccionMovimiento();

        // Instanciar una nueva bomba
        if (Input.GetKeyDown(teclaColocar) && HerramientaActiva == "Bomba")
        {
            InstanciarBomba();
        }

        // Interactuar con bombas existentes
        if (Input.GetKeyDown(teclaInteractuar) && HerramientaActiva == "Bomba")
        {
            if (sosteniendoBomba)
            {
                LanzarBomba();
            }
            else
            {
                AgarrarBomba();
            }
        }
    }

    private void CapturarDireccionMovimiento()
    {
        Vector3 movimiento = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (movimiento != Vector3.zero)
        {
            ultimaDireccion = movimiento;
        }
    }

    private void InstanciarBomba()
    {
        if (InventoryManager.instance.HasItem("Bomba") && InventoryManager.instance.PuedoUsarItem(InventoryManager.instance.items.Find(i => i.itemName == "Bomba"))) // Verificar si hay bombas en el inventario
        {
            InventoryManager.instance.UsarItem(InventoryManager.instance.items.Find(i => i.itemName == "Bomba")); // Reducir cantidad
            Vector3 posicionBomba = transform.position; // Instanciar en los pies del jugador
            GameObject nuevaBomba = Instantiate(prefabBomba, posicionBomba, Quaternion.identity);
            Debug.Log("Bomba colocada en el suelo.");
        }
        else
        {
            Debug.Log("No tienes bombas disponibles.");
        }
    }

    private void AgarrarBomba()
    {
        // Detectar bombas cercanas
        Collider[] bombasCercanas = Physics.OverlapSphere(transform.position, radioDeteccion);
        foreach (var col in bombasCercanas)
        {
            Bombas bomba = col.GetComponent<Bombas>();
            if (bomba != null)
            {
                bombaActual = bomba;
                bombaActual.OnBombaExplotada += BombaExplotada; // Suscribirse al evento
                bombaActual.transform.SetParent(posicionSostener); // Fijar la bomba al jugador
                bombaActual.transform.localPosition = Vector3.zero; // Centrarla en la posición de sostener
                bombaActual.GetComponent<Rigidbody>().isKinematic = true; // Desactivar la física
                bombaActual.GetComponent<Collider>().enabled = false; // Desactivar la física
                sosteniendoBomba = true;
                Debug.Log("Bomba agarrada.");
                break;
            }
        }
    }

    private void LanzarBomba()
    {
        if (bombaActual != null)
        {
            bombaActual.OnBombaExplotada -= BombaExplotada; // Desuscribirse del evento
            bombaActual.transform.SetParent(null); // Liberar la bomba del jugador
            Rigidbody rb = bombaActual.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Reactivar la física
                bombaActual.GetComponent<Collider>().enabled = true; // Desactivar la física
                rb.AddForce(ultimaDireccion * fuerzaLanzamiento, ForceMode.Impulse); // Lanzar hacia la última dirección
            }
            bombaActual = null;
            sosteniendoBomba = false;
            Debug.Log("Bomba lanzada.");
        }
    }

    private void BombaExplotada(Bombas bomba)
    {
        if (bomba == bombaActual)
        {
            Debug.Log("La bomba explotó mientras la sostenías.");
            bombaActual = null;
            sosteniendoBomba = false;
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
