using UnityEngine;
using System.Collections;

public class Ataque : MonoBehaviour
{
    private Animator anim;
    public bool atkCooldown = true;

    private Movimiento movimientoScript;
    private DisparoFlecha ArcoScript;
    private Gancho GanchoScript;
    private EstaminaJugador estaminaJugador;
    private SelectorDeMagia SelectorDeMagia;


    private bool isAttacking = false;
    private bool isChargingAttack = false;

    public float estaminaPorAtaque = 20f;
    public float estaminaPorAtaqueMax = 75f;


    private Vector3 moveDirection;

    // Variables para el ataque cargado
    public float tiempoCargaMax = 3f;
    private float tiempoCargando = 0f;
    private float duracionAtaqueBase = 1f;
    private float duracionAtaqueMax = 5f;
    [SerializeField] string currentWeapon = "Espada";
    private void Start()
    {
        anim = GetComponent<Animator>();
        estaminaJugador = GetComponent<EstaminaJugador>();
        movimientoScript = GetComponent<Movimiento>();
        ArcoScript = GetComponent<DisparoFlecha>();
        GanchoScript = GetComponent<Gancho>();
        SelectorDeMagia = GetComponent<SelectorDeMagia>();
    }

    private void Update()
    {
        if (!isAttacking && !isChargingAttack)
        {
            if (movimientoScript.movement != Vector3.zero)
            {
                moveDirection = movimientoScript.movement;
            }

            anim.SetFloat("MoveX", moveDirection.x);
            anim.SetFloat("MoveZ", moveDirection.z);
        }

        // Ataque normal con la tecla "J"
        if (Input.GetKeyDown(KeyCode.J) && atkCooldown && !isAttacking && InventoryManager.instance.HasItem(currentWeapon))
        {
            anim.SetTrigger("Atk");
            atkCooldown = false;
            isAttacking = true;

            movimientoScript.enabled = false;
            ArcoScript.enabled = false;
            GanchoScript.enabled = false;
            SelectorDeMagia.enabled = false;
        }

        // Ataque cargado con la tecla "K"
        if (Input.GetKey(KeyCode.K) && atkCooldown && !isAttacking)
        {
            isChargingAttack = true;
            tiempoCargando += Time.deltaTime;
            tiempoCargando = Mathf.Clamp(tiempoCargando, 0, tiempoCargaMax);
        }

        if (Input.GetKeyUp(KeyCode.K) && isChargingAttack && InventoryManager.instance.HasItem(currentWeapon) && InventoryManager.instance.IsAbilityUnlocked("Girar"))
        {
            float estaminaNecesaria = tiempoCargando >= tiempoCargaMax ? estaminaPorAtaqueMax : estaminaPorAtaque;

            // Verificar si hay suficiente estamina antes de disparar
            if (estaminaJugador.UsarEstamina(estaminaNecesaria))
            {
                // Calcular la duración del ataque basado en el tiempo de carga
                float duracionAtaque = Mathf.Lerp(duracionAtaqueBase, duracionAtaqueMax, tiempoCargando / tiempoCargaMax);
                anim.SetBool("DuracionAtaque", false);
                anim.SetTrigger("AtaqueCargado");

                atkCooldown = false;
                isChargingAttack = false;
                isAttacking = true;

                movimientoScript.enabled = false;
                ArcoScript.enabled = false;
                GanchoScript.enabled = false;
                SelectorDeMagia.enabled = false;

                tiempoCargando = 0;

                // Iniciar la corrutina para controlar la duración del ataque cargado
                StartCoroutine(FinalizarAtaqueCargado(duracionAtaque));
            }
            else
            {
                ResetCooldown();
            }
        }
    }

    // Corrutina para esperar la duración del ataque cargado antes de reactivar el movimiento y ataques
    private IEnumerator FinalizarAtaqueCargado(float duracionAtaque)
    {
        Debug.Log("se inicio la corrutina");
        yield return new WaitForSeconds(duracionAtaque);
        ResetCooldown();
    }

    public void ResetCooldown()
    {
        anim.ResetTrigger("Atk");
        anim.SetBool("DuracionAtaque", true);

        atkCooldown = true;
        isAttacking = false;
        isChargingAttack = false;

        movimientoScript.enabled = true;
        ArcoScript.enabled = true;
        GanchoScript.enabled = true;
        SelectorDeMagia.enabled = true;
    }
}
