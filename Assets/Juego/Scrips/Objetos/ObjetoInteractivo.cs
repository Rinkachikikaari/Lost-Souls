using UnityEngine;
using UnityEngine.Rendering;

public class ObjetoInteractivo : MonoBehaviour
{
    [Header("Propiedades")]
    public float peso = 1.0f;
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
            Debug.Log(direccion);
            rb.MovePosition(rb.position + direccion);
        }
    }

    public void Tomar()
    {
        rb.isKinematic = false;
    }
    public void Soltar()
    {
        rb.isKinematic = true;
    }
}
