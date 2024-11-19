using UnityEngine;

public class LightningSpell : MonoBehaviour
{
    public float costoMana = 20f;
    public float duracionAturdimiento = 2f;
    public float rango = 10f;
    public LayerMask enemigosLayer;

    private Magia manaSystem;
    private Vector3 ultimaDireccion = Vector3.right; // Direcci�n inicial (derecha por defecto)
    private Movimiento movimientoScript; // Referencia al script de movimiento

    private void Start()
    {
        manaSystem = GetComponent<Magia>();
        movimientoScript = GetComponent<Movimiento>();
    }

    private void Update()
    {
        // Actualizar la �ltima direcci�n si el jugador se est� moviendo
        if (movimientoScript.movement != Vector3.zero)
        {
            ultimaDireccion = movimientoScript.movement.normalized;
        }
    }

    public void LanzarRayo()
    {
        if (!manaSystem.UsarMana(costoMana)) return; // Verificar si hay suficiente man�

        // Realizar el raycast en la �ltima direcci�n guardada
        RaycastHit hit;
        if (Physics.Raycast(transform.position, ultimaDireccion, out hit, rango, enemigosLayer))
        {
            Debug.Log("Impacto con " + hit.collider.name);

            Enemy enemigo = hit.collider.GetComponent<Enemy>();
            if (enemigo != null && enemigo.puedeSerAturdido)
            {
                enemigo.Aturdir(duracionAturdimiento);
                Debug.Log($"{hit.collider.name} ha sido aturdido.");
            }
        }

        // Visualizaci�n de depuraci�n
        Debug.DrawLine(transform.position, transform.position + ultimaDireccion * rango, Color.yellow, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + ultimaDireccion * rango);
        Gizmos.DrawWireSphere(transform.position + ultimaDireccion * rango, 0.2f);
    }
}
