using UnityEngine;

public class DañoAtaque : MonoBehaviour
{
    public int Daño;
    static public bool Giratorio;
    public void Update()
    {
        Daño = PlayerStats.instance.espadaEquipada.Daño;

    }
    private void OnTriggerEnter(Collider other)
    {
        // Solo afecta a los enemigos
        if (other.CompareTag("Enemigo"))
        {
            // Obtener el script del enemigo y aplicar daño
            Enemy enemigo = other.GetComponent<Enemy>();
            if (enemigo != null)
            {
                if (!Giratorio)
                {
                    enemigo.RecibirDano(Daño);
                }
                if (Giratorio)
                {
                    enemigo.RecibirDano(Daño + 10);
                }
            }
        }
    }
}
