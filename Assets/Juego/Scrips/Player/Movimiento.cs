using System;
using System.Collections;
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
    [SerializeField] int dashTileMove = 2;
    private float lastDashTime = -Mathf.Infinity;
    bool isMoving = false;
    float currentMove = 0;
    Vector3 initialMovePosition = Vector3.zero;
    [SerializeField] Vector3 realOffset = Vector3.zero;
    Coroutine moveC = null;

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
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= lastDashTime + dashCooldown && movement.magnitude > 0)
        {
            if(moveC != null)
            {
                transform.position = new Vector3(initialMovePosition.x, transform.position.y, initialMovePosition.z) + movement;
            }
            
            isDashing = true;
            // dashTime = dashDuration;
            lastDashTime = Time.time;
            DashEn4Direcciones();
        }
    }

    void FixedUpdate()
    {
        // Aplicar movimiento normal o de dash
        if (isDashing)
        {
            // rb.MovePosition(rb.position + movement * dashSpeed * Time.fixedDeltaTime);
            // dashTime -= Time.fixedDeltaTime;

            // Finalizar el dash cuando se acabe el tiempo
            // if (dashTime <= 0)
            // {
            //     isDashing = false;
            // }
        }
        else
        {
            // rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
            MoverEn4Direcciones();
        }
    }


    private void MoverEn4Direcciones()
    {
        if (isMoving == false && movement.magnitude > 0)
        {
            currentMove = 0;
            initialMovePosition = transform.position;
            moveC = StartCoroutine(MoveDirection(movement));
        }
    }

    private void DashEn4Direcciones()
    {
        if (isDashing && movement.magnitude > 0)
        {
            currentMove = 0;
            initialMovePosition = transform.position;
            StartCoroutine(DashDirection(movement));
        }
    }

    IEnumerator MoveDirection(Vector3 direccionMovimiento)
    {
        isMoving = true;

        while (currentMove < 1)
        {
            yield return -1;
            currentMove += direccionMovimiento.magnitude * speed * Time.deltaTime;
            transform.position += direccionMovimiento * speed * Time.deltaTime;
            
        }
        transform.position = new Vector3(initialMovePosition.x, transform.position.y, initialMovePosition.z) + direccionMovimiento;
        isMoving = false;
        moveC = null;
    }

    IEnumerator DashDirection(Vector3 direccionMovimiento)
    {
        if (moveC != null)
        {
            StopCoroutine(moveC);
            isMoving = false;
        }
        

        isDashing = true;
        float totalDashMove = dashTileMove;


        yield return -1;

        
        while (currentMove < totalDashMove)
        {
            yield return -1;
            currentMove += direccionMovimiento.magnitude * dashSpeed * Time.deltaTime;
            transform.position += direccionMovimiento * dashSpeed * Time.deltaTime;
        }
        

        Debug.Log(totalDashMove);
        Debug.Log(new Vector3(initialMovePosition.x, transform.position.y, initialMovePosition.z) + ", " + (new Vector3(initialMovePosition.x, transform.position.y, initialMovePosition.z) + direccionMovimiento * totalDashMove));

        transform.position = new Vector3(initialMovePosition.x, transform.position.y, initialMovePosition.z) + direccionMovimiento * totalDashMove;

        isDashing = false;
    }

}
