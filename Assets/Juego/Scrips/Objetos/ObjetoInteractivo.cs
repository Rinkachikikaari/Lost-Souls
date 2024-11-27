using UnityEngine;

public class ObjetoInteractivo : MonoBehaviour
{
    [Header("Propiedades")]
    public bool esEmpujable = true; // Si el objeto puede ser empujado
    public bool esArrastrable = true; // Si el objeto puede ser arrastrado

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"El objeto {name} necesita un Rigidbody para ser interactivo.");
        }
    }

    public void Mover(Vector3 direccion)
    {
        if (rb != null)
        {
            rb.linearVelocity = direccion; // Mueve el objeto en la dirección especificada
        }
    }
}
