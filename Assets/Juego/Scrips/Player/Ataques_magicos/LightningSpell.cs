using UnityEngine;

public class LightningSpell : MonoBehaviour
{
    public float costoMana = 20f;
    public float duracionAturdimiento = 2f;
    public float rango = 10f;
    public LayerMask enemigosLayer;

    private Magia manaSystem;

    private void Start()
    {
        manaSystem = GetComponent<Magia>();
    }

    private void Update()
    {

    }

    public void LanzarRayo()
    {
        if (!manaSystem.UsarMana(costoMana)) return; // Verificar si hay suficiente maná

        // Crear un rayo hacia adelante
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rango, enemigosLayer))
        {
            Debug.Log("Impacto con " + hit.collider.name);

            // Verificar si el enemigo puede ser aturdido
            Enemy enemigo = hit.collider.GetComponent<Enemy>();
            if (enemigo != null && enemigo.puedeSerAturdido)
            {
                enemigo.Aturdir(duracionAturdimiento);
                Debug.Log($"{hit.collider.name} ha sido aturdido.");
            }
            else
            {
                Debug.Log($"{hit.collider.name} no puede ser aturdido.");
            }
        }
    }
}
