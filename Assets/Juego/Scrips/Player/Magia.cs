using UnityEngine;

public class Magia : MonoBehaviour
{
    public float manaMaxima = 100f;
    public float regeneracionPorSegundo = 5f;
    public float tiempoEsperaRegeneracion = 2f;

    private float manaActual;
    private float tiempoUltimoGasto;

    private void Start()
    {
        manaActual = manaMaxima; // Inicializa con el maná máximo
    }

    private void Update()
    {
        RegenerarMana();
    }

    private void RegenerarMana()
    {
        // Si ha pasado suficiente tiempo desde el último gasto, comienza a regenerar
        if (Time.time >= tiempoUltimoGasto + tiempoEsperaRegeneracion)
        {
            manaActual = Mathf.Clamp(manaActual + regeneracionPorSegundo * Time.deltaTime, 0, manaMaxima);
        }
    }

    public bool UsarMana(float cantidad)
    {
        if (manaActual >= cantidad)
        {
            manaActual -= cantidad;
            tiempoUltimoGasto = Time.time; // Registrar el tiempo del último uso de maná
            return true; // Se pudo usar el maná
        }
        return false; // No hay suficiente maná
    }

    public float ObtenerManaActual()
    {
        return manaActual;
    }
}