using UnityEngine;

public class Flecha : MonoBehaviour
{
    public float tiempoDeVida = 5f;      // Tiempo que la flecha permanecer� en la escena despu�s de ser disparada

    void Start()
    {
        // Destruir la flecha despu�s de cierto tiempo para optimizar recursos
        Destroy(gameObject, tiempoDeVida);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verificar si la flecha ha impactado contra el suelo, una superficie o un enemigo
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            // Acciones al golpear un enemigo, por ejemplo, infligir da�o
            // Aqu� podr�as acceder a un script del enemigo para reducir su salud
            //collision.gameObject.GetComponent<Enemigo>()?.TomarDano(10); // Asumiendo que tienes un m�todo TomarDano
            DestruirFlecha();
        }
        else if (collision.gameObject.CompareTag("Superficie") || collision.gameObject.CompareTag("Suelo"))
        {
            // Al golpear cualquier otra superficie, simplemente se "clava" y se detiene
            ClavarFlecha();
        }
        else
        {
            // Al golpear otros objetos, la flecha se destruye
            DestruirFlecha();
        }
    }

    void ClavarFlecha()
    {
        // Detener la f�sica de la flecha para que quede "clavada" en la superficie
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = null;  // Asegurar que no est� parentada a otros objetos
        Destroy(this);            // Opcional: desactivar este script para evitar m�s colisiones
    }

    void DestruirFlecha()
    {
        // Destruir la flecha al golpear un enemigo u objeto espec�fico
        Destroy(gameObject);
    }
}
