using UnityEngine;

public class Retroceso : MonoBehaviour
{
    public float retrocesoFuerza = 5f; // Fuerza del retroceso para el enemigo

    // M�todo para manejar la colisi�n con el enemigo
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo"))
        {
            // Calcular la normal de retroceso entre el jugador y el enemigo
            Vector3 direccionRetroceso = CalcularDireccionRetroceso(other.transform.position);

            // Aplicar la fuerza de retroceso al enemigo en la direcci�n calculada
            Rigidbody enemigoRb = other.GetComponent<Rigidbody>();
            if (enemigoRb != null)
            {
                enemigoRb.AddForce(direccionRetroceso * retrocesoFuerza, ForceMode.Impulse);
            }
        }
    }

    // M�todo para calcular la direcci�n de retroceso redondeada
    private Vector3 CalcularDireccionRetroceso(Vector3 posicionEnemigo)
    {
        Vector3 direccion = (posicionEnemigo - transform.position).normalized;

        // Redondear la direcci�n a una de las 4 direcciones principales
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
        {
            // Retroceso en el eje X
            return new Vector3(Mathf.Sign(direccion.x), 0, 0);
        }
        else
        {
            // Retroceso en el eje Z
            return new Vector3(0, 0, Mathf.Sign(direccion.z));
        }
    }
}
