using UnityEngine;

public class AirSpell : MonoBehaviour
{
    public float costoMana = 25f;
    public float radio = 8f;
    public float fuerzaEmpuje = 10f;
    public LayerMask enemigosLayer;

    private Magia manaSystem;

    private void Start()
    {
        manaSystem = GetComponent<Magia>();
    }

    private void Update()
    {

    }

    public void LanzarAire()
    {
        if (!manaSystem.UsarMana(costoMana)) return; // Verificar si hay suficiente maná

        Collider[] enemigos = Physics.OverlapSphere(transform.position, radio, enemigosLayer);
        foreach (Collider enemigo in enemigos)
        {
            Enemy enemyScript = enemigo.GetComponent<Enemy>();
            if (enemyScript != null && enemyScript.puedeSerEmpujado)
            {
                Rigidbody rb = enemigo.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    enemyScript.SerEmpujado(); // Empujar al enemigo
                    enemyScript.Aturdir(2f); // Detener la IA por 2 segundos
                    Vector3 direccionEmpuje = (enemigo.transform.position - transform.position).normalized;
                    rb.AddForce(direccionEmpuje * fuerzaEmpuje, ForceMode.Impulse);
                }
            }
        }

        Debug.Log("Lanzando aire!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
