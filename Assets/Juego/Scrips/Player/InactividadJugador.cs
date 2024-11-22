using UnityEngine;

public class InactividadJugador : MonoBehaviour
{
    public Animator animator;         // Referencia al Animator
    public float tiempoInactividad = 10f; // Tiempo límite para detectar inactividad
    private float contadorInactividad = 0f;
    private bool estaDormido = false;

    void Update()
    {
        // Detectar si hay entrada del jugador
        if (Input.anyKey || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if (estaDormido)
            {
                // Si estaba "dormido", cambiar a estado activo
                animator.SetInteger("dormir", 0);
                estaDormido = false;
            }

            // Reiniciar contador de inactividad
            contadorInactividad = 0f;
        }
        else
        {
            // Incrementar el contador si no hay entrada
            contadorInactividad += Time.deltaTime;

            // Si supera el tiempo de inactividad, activar "dormir"
            if (contadorInactividad >= tiempoInactividad && !estaDormido)
            {
                animator.SetInteger("dormir", 1);
                estaDormido = true;
            }
        }
    }
}
