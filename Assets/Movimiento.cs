using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float speed = 5f;           // Velocidad de movimiento
    public float dashSpeed = 15f;      // Velocidad durante el dash
    public float dashDuration = 0.2f;  // Duración del dash en segundos
    public float dashCooldown = 1f;    // Tiempo de recarga del dash en segundos

    private Rigidbody rb;
    public Vector3 movement;
    private bool isDashing = false;
    private float dashTime;
    private float lastDashTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Obtener entrada del jugador en los ejes X y Z
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Establecer el vector de movimiento en el plano XZ
        if (horizontal != 0)
        {
            movement = new Vector3(horizontal, 0, 0).normalized;
        }
        else
        {
            movement = new Vector3(0, 0, vertical).normalized;
        }

        // Verificar si el jugador inicia un dash
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            isDashing = true;
            dashTime = dashDuration;
            lastDashTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        // Aplicar movimiento normal o de dash
        if (isDashing)
        {
            rb.MovePosition(rb.position + movement * dashSpeed * Time.fixedDeltaTime);
            dashTime -= Time.fixedDeltaTime;

            // Finalizar el dash cuando se acabe el tiempo
            if (dashTime <= 0)
            {
                isDashing = false;
            }
        }
        else
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }
}
