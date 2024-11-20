using UnityEngine;

public class Fragmento : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FragmentosCorazon fragmentosCorazon = other.GetComponent<FragmentosCorazon>();
            if (fragmentosCorazon != null)
            {
                fragmentosCorazon.AgregarFragmento();
                Destroy(gameObject); // Destruir el fragmento después de recogerlo
            }
        }
    }
}
