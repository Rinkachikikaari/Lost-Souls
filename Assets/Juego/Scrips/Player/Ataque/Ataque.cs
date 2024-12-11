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

    public int estaminaPorAtaque;
    public int estaminaPorAtaqueMax;


    private Vector3 moveDirection;

    // Variables para el ataque cargado
    public int tiempoCargaMax;
    private float tiempoCargando = 0;
    public int duracionAtaqueBase;
    public int duracionAtaqueMax;
    [SerializeField] string currentWeapon;
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
        if (Input.GetKeyDown(KeyCode.J) && atkCooldown && !isAttacking && InventoryManager.instance.HasEquip(currentWeapon))
        {
            anim.SetTrigger(currentWeapon);
            atkCooldown = false;
            isAttacking = true;

            movimientoScript.enabled = false;
            ArcoScript.enabled = false;
            GanchoScript.enabled = false;
            SelectorDeMagia.enabled = false;
        }

        // Ataque cargado con la tecla "K"
        if (Input.GetKey(KeyCode.K) && atkCooldown && !isAttacking && InventoryManager.instance.HasEquip(currentWeapon) && InventoryManager.instance.IsAbilityUnlocked("Girar"))
        {
            // Verificar si hay suficiente estamina antes de disparar
            if (estaminaJugador.UsarEstamina(estaminaPorAtaqueMax))
            {
                
                anim.SetTrigger(currentWeapon + "Cargado");

                atkCooldown = false;
                isAttacking = true;

                movimientoScript.enabled = false;
                ArcoScript.enabled = false;
                GanchoScript.enabled = false;
                SelectorDeMagia.enabled = false;


            }
            else
            {
                ResetCooldown();
            }
        }
    }
    public void UpdateCurrentWeapon()
    {
        if (PlayerStats.instance.espadaEquipada != null)
        {
            currentWeapon = PlayerStats.instance.espadaEquipada.EquipName;
            estaminaPorAtaque = PlayerStats.instance.espadaEquipada.CostoDeStaminaMin;
            estaminaPorAtaqueMax = PlayerStats.instance.espadaEquipada.CostoDeStaminaMax;
            tiempoCargaMax = PlayerStats.instance.espadaEquipada.TiempoDeCarga;
            duracionAtaqueBase = PlayerStats.instance.espadaEquipada.DuracionDeAtaqueBase;
            duracionAtaqueMax = PlayerStats.instance.espadaEquipada.DuracionDeAtaqueMax;

        }
        else
        {
            currentWeapon = "Sin arma equipada";
        }

        Debug.Log($"Arma actualizada: {currentWeapon}");
    }

    public void ResetCooldown()
    {
        anim.ResetTrigger(currentWeapon);

        atkCooldown = true;
        isAttacking = false;
        isChargingAttack = false;

        movimientoScript.enabled = true;
        ArcoScript.enabled = true;
        GanchoScript.enabled = true;
        SelectorDeMagia.enabled = true;
    }
}
