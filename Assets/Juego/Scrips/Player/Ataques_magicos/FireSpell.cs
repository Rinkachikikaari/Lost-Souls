using UnityEngine;

public class FireSpell : MonoBehaviour
{
    public float costoMana = 30f;
    public float radio = 5f;
    public float dano = 50f;
    public LayerMask enemigosLayer;

    private Magia manaSystem;

    private void Start()
    {
        manaSystem = GetComponent<Magia>();
    }

    private void Update()
    {

    }

    public void LanzarFuego()
    {
        if (!manaSystem.UsarMana(costoMana)) return; // Verificar si hay suficiente maná

        // Crear una onda expansiva alrededor del jugador
        Collider[] enemigos = Physics.OverlapSphere(transform.position, radio, enemigosLayer);
        foreach (Collider enemigo in enemigos)
        {
            Enemy enemyScript = enemigo.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.RecibirDano(dano);
                Debug.Log($"{enemigo.name} recibió {dano} de daño.");
            }
        }

        Debug.Log("Lanzando fuego!");
    }
}
