using UnityEngine;
using UnityEngine.UI;

public class BarraUI : MonoBehaviour
{
    [Header("Barras de UI")]
    public Slider barraStamina;
    public Slider barraMana;

    [Header("Referencias al jugador")]
    public EstaminaJugador staminaSystem;
    public Magia manaSystem;

    private void Start()
    {
        // Inicializar barras
        ActualizarBarras();
    }

    private void Update()
    {
        // Actualizar las barras cada cuadro
        ActualizarBarras();
    }

    private void ActualizarBarras()
    {
        if (staminaSystem != null)
        {
            barraStamina.value = staminaSystem.ObtenerEstaminaActual() / staminaSystem.estaminaMaxima;
        }

        if (manaSystem != null)
        {
            barraMana.value = manaSystem.ObtenerManaActual() / manaSystem.manaMaxima;
        }
    }
}
