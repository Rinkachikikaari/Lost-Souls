using System.Collections;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float speed = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody rb;
    private Animator animator;
    public Vector3 movement;
    private bool isDashing = false;
    [SerializeField] int dashTileMove = 2;
    private float lastDashTime = -Mathf.Infinity;
    private bool isMovingLocked = false; // Indica si el movimiento está bloqueado
    private KeyCode lockedKey;         // Tecla bloqueada para el movimiento
    private Vector3 lockedDirection = Vector3.zero; // Dirección bloqueada

    private float lastMoveX = 0;       // Última dirección en X
    private float lastMoveY = 0;       // Última dirección en Y
    public JugadorInteraccion ji;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Detectar entrada de movimiento
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Determinar la tecla presionada y la dirección correspondiente
        if (!isMovingLocked)
        {
            if (horizontal > 0)
            {
                lockedKey = KeyCode.D;
                lockedDirection = Vector3.right;
            }
            else if (horizontal < 0)
            {
                lockedKey = KeyCode.A;
                lockedDirection = Vector3.left;
            }
            else if (vertical > 0)
            {
                lockedKey = KeyCode.W;
                lockedDirection = Vector3.forward;
            }
            else if (vertical < 0)
            {
                lockedKey = KeyCode.S;
                lockedDirection = Vector3.back;
            }

            // Bloquear la dirección si hay movimiento
            if (lockedDirection != Vector3.zero)
            {
                isMovingLocked = true;
            }
        }

        // Desbloquear si la tecla bloqueada se libera
        if (isMovingLocked && !Input.GetKey(lockedKey))
        {
            isMovingLocked = false;
            lockedDirection = Vector3.zero;
        }

        // Establecer el movimiento solo en la dirección bloqueada
        movement = isMovingLocked ? lockedDirection : Vector3.zero;

        // Actualizar el Animator
        if (movement.magnitude > 0)
        {
            lastMoveX = movement.x;
            lastMoveY = movement.z;

            animator.SetFloat("MoveX", movement.x);
            animator.SetFloat("MoveZ", movement.z);
        }
        else
        {
            // Mantener la última dirección
            animator.SetFloat("MoveX", lastMoveX);
            animator.SetFloat("MoveZ", lastMoveY);
        }

        // Actualizar la velocidad en el Animator
        animator.SetFloat("Speed", movement.magnitude);

        // Detectar si el jugador intenta hacer dash
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= lastDashTime + dashCooldown && movement.magnitude > 0)
        {
            isDashing = true;
            lastDashTime = Time.time;
            DashEn4Direcciones();
        }
    }

    void FixedUpdate()
    {
        if (!isDashing && movement.magnitude > 0)
        {

            if (ji.agarrando)
            {
                Vector3 movimiento = movement * (speed / ji.PesoObjeto()) * Time.fixedDeltaTime ;
                

                ji.MoveInteraction(movimiento);
                rb.MovePosition(rb.position + movimiento);
            }
            else { 
                rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
            }
        }
    }

    private void DashEn4Direcciones()
    {
        if (isDashing && movement.magnitude > 0)
        {
            StartCoroutine(DashDirection(movement));
        }
    }

    IEnumerator DashDirection(Vector3 direccionMovimiento)
    {
        isDashing = true;

        animator.SetTrigger("Roll"); // Activar el trigger de animación

        float elapsedTime = 0f;
        Vector3 startPosition = rb.position;
        Vector3 targetPosition = startPosition + direccionMovimiento * dashSpeed * dashDuration;

        // Movimiento interpolado
        while (elapsedTime < dashDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / dashDuration;

            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
            rb.MovePosition(newPosition);

            yield return null;
        }

        isDashing = false;
    }
}
