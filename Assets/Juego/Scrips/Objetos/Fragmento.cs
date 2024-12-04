using UnityEngine;

public class Fragmento : MonoBehaviour
{
    [Header("Configuración del Fragmento")]
    public int cantidadFragmentos = 1; // Cantidad de fragmentos que otorga este objeto
    private bool entre = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!entre)
            {
                FragmentosCorazon fragmentosCorazon = other.GetComponentInParent<FragmentosCorazon>();
                if (fragmentosCorazon != null)
                {
                    fragmentosCorazon.AgregarFragmento(cantidadFragmentos);
                    entre = true;   
                    Destroy(gameObject); // Destruir el fragmento después de recogerlo
                }
            }
        }
    }
}
