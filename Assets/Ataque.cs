using UnityEngine;

public class Ataque : MonoBehaviour
{
    private Animator anim;
    public bool atkCooldown = true;

    private Vector3 moveDirection;
    private Movimiento movimientoScript; // Referencia al script de movimiento

    private void Start()
    {
        anim = GetComponent<Animator>();
        movimientoScript = GetComponent<Movimiento>(); // Aseg�rate de tener el script de movimiento en el mismo objeto
    }

    private void Update()
    {
        // Capturar la direcci�n de movimiento actual desde el script de movimiento
        moveDirection = movimientoScript.movement;

        // Pasar la direcci�n de movimiento al Animator
        anim.SetFloat("MoveX", moveDirection.x);
        anim.SetFloat("MoveZ", moveDirection.z);

        // Verificar entrada de ataque y cooldown
        if (Input.GetKeyDown(KeyCode.J) && atkCooldown)
        {
            anim.SetTrigger("Atk");
            atkCooldown = false;
        }
    }

    // M�todo llamado desde el evento de animaci�n para reiniciar el cooldown
    public void ResetCooldown()
    {
        atkCooldown = true;
    }
}

