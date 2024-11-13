using UnityEngine;

public class Ataque : MonoBehaviour
{
    private Animator anim;
    public bool atkCooldown = true;

    private Movimiento movimientoScript; // Referencia al script de movimiento
    private bool isAttacking = false;    // Controla si el personaje est� atacando

    private Vector3 moveDirection;

    private void Start()
    {
        anim = GetComponent<Animator>();
        movimientoScript = GetComponent<Movimiento>(); // Aseg�rate de tener el script de movimiento en el mismo objeto
    }

    private void Update()
    {
        // Guardar la direcci�n de movimiento antes de iniciar el ataque
        if (!isAttacking)
        {
        // Guardar la �ltima direcci�n de movimiento si el jugador se est� moviendo
        if (movimientoScript.movement != Vector3.zero)
        {
            moveDirection = movimientoScript.movement;
        }

            // Pasar la direcci�n de movimiento al Animator
            anim.SetFloat("MoveX", moveDirection.x);
            anim.SetFloat("MoveZ", moveDirection.z);
        }

        // Verificar entrada de ataque y cooldown
        if (Input.GetKeyDown(KeyCode.J) && atkCooldown && !isAttacking)
        {
            anim.SetTrigger("Atk");
            atkCooldown = false;
            isAttacking = true;

            // Desactivar el movimiento durante el ataque
            movimientoScript.enabled = false;
        }
    }

    // M�todo llamado desde el evento de animaci�n para reiniciar el cooldown y el movimiento
    public void ResetCooldown()
    {
        atkCooldown = true;
        isAttacking = false; // Permitir movimiento nuevamente

        // Reactivar el movimiento cuando el ataque termine
        movimientoScript.enabled = true;
    }
}
