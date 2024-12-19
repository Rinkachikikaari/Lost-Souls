using UnityEngine;

public class InteraccionConObjeto : MonoBehaviour
{
    public string objetoRequerido; // Nombre del objeto necesario en el inventario
    public GameObject objetoActivado; // Objeto que se activará (puerta, mecanismo, etc.)

    public bool Activar;
    public bool Desactivar;
    private bool jugadorCerca = false;

    void Update()
    {
        // Verifica si el jugador está cerca y presiona la tecla E
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            // Comprueba si el inventario tiene el objeto requerido
            if (InventoryManager.instance.HasItem(objetoRequerido))
            {
                // Elimina el objeto del inventario
                ItemData item = InventoryManager.instance.items.Find(i => i.itemName == objetoRequerido);
                if (item != null)
                {
                    InventoryManager.instance.UsarItem(item);
                    Debug.Log($"Se ha usado el objeto: {objetoRequerido}");
                }

                // Activa el objeto asociado
                if (objetoActivado != null)
                {
                    if (Activar)
                    {
                        objetoActivado.SetActive(true);
                        Debug.Log("Objeto " + Activar);

                    }
                    if (Desactivar)
                    {
                        objetoActivado.SetActive(false);
                        Debug.Log("Objeto " + Desactivar);

                    }

                }
            }
            else
            {
                Debug.Log("No tienes el objeto requerido.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            Debug.Log("Jugador cerca del objeto.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            Debug.Log("Jugador se alejó del objeto.");
        }
    }
}
