using UnityEngine;

public class TiendaObjeto : MonoBehaviour
{
    [Header("Información del Objeto")]
    public ItemData item; // El ítem en venta
    public GameObject panelInteraccion; // Panel de interacción
    public int Cantidad;
    public UnityEngine.UI.Text textoInteraccion; // Texto para mostrar la información del ítem
    public SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer del GameObject (para 2D)

    private bool comprado = false;
    private bool cercaDelObjeto = false;

    private void Start()
    {
        if (panelInteraccion != null)
        {
            panelInteraccion.SetActive(false); // El panel está oculto al inicio
        }
        ActualizarSprite();
    }
    private void ActualizarSprite()
    {
        if (item != null)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = item.icon; // Cambia el sprite en el mundo 2D
            }
        }
        else
        {
            Debug.LogWarning("El ítem no está configurado en este objeto de la tienda.");
        }
    }
    private void Update()
    {
        if (cercaDelObjeto && Input.GetKeyDown(KeyCode.E))
        {
            MostrarDetallesObjeto();
        }
        else if (cercaDelObjeto && Input.GetKeyDown(KeyCode.Y))
        {
            ComprarObjeto();
        }
        else if (cercaDelObjeto && Input.GetKeyDown(KeyCode.N))
        {
            CerrarInteraccion();
        }
    }

    private void MostrarDetallesObjeto()
    {
        if (panelInteraccion != null && textoInteraccion != null && item != null)
        {
            panelInteraccion.SetActive(true);
            textoInteraccion.text = $"¿Quieres comprar {item.itemName}?\n" +
                                    $"Cantidad: {Cantidad}\n" +
                                    $"Precio: {item.precio * Cantidad} monedas\n" +
                                    "[Y] Sí    [N] No";
        }
    }

    private void ComprarObjeto()
    {
        if (item == null || comprado) return;

        // Acceder a las monedas desde el InventoryManager
        ItemData monedaItem = InventoryManager.instance.items.Find(i => i.subCategory == ItemSubCategory.Moneda);
        if (monedaItem != null && monedaItem.cantidad >= item.precio)
        {
            monedaItem.cantidad -= item.precio * Cantidad;
            InventoryManager.instance.AddItem(item, Cantidad);

            textoInteraccion.text = "¡Gracias por su compra!";
            comprado = true;
            gameObject.SetActive(false); // Desactiva el objeto tras la compra
            Invoke("CerrarInteraccion", 2f);
        }
        else
        {
            textoInteraccion.text = "No tienes suficientes monedas.";
            Invoke("CerrarInteraccion", 2f);
        }
    }

    private void CerrarInteraccion()
    {
        if (panelInteraccion != null)
        {
            panelInteraccion.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cercaDelObjeto = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cercaDelObjeto = false;
            CerrarInteraccion();
        }
    }
}
