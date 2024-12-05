using UnityEngine;

public class EstructuraDestruible : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject efectoDestruccion; // Efecto visual cuando se destruye
    public bool usarFisicaFragmentos = true; // Si los fragmentos deben usar física al destruirse

    public void Destruir()
    {
        // Mostrar efecto de destrucción si existe
        if (efectoDestruccion != null)
        {
            Instantiate(efectoDestruccion, transform.position, Quaternion.identity);
        }

        // Destruir el objeto principal
        if (usarFisicaFragmentos)
        {
            // Separar fragmentos si el objeto tiene hijos
            foreach (Transform fragmento in transform)
            {
                Rigidbody rb = fragmento.gameObject.AddComponent<Rigidbody>();
                rb.AddExplosionForce(300f, transform.position, 5f); // Fuerza de explosión en fragmentos
                fragmento.SetParent(null); // Liberar fragmentos del padre
            }
        }

        Destroy(gameObject); // Eliminar el objeto principal
    }
}
