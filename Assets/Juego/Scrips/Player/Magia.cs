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
        manaActual = manaMaxima; // Inicializa con el man� m�ximo
    }

    private void Update()
    {
        RegenerarMana();
    }

    private void RegenerarMana()
    {
        // Si ha pasado suficiente tiempo desde el �ltimo gasto, comienza a regenerar
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
            tiempoUltimoGasto = Time.time; // Registrar el tiempo del �ltimo uso de man�
            return true; // Se pudo usar el man�
        }
        return false; // No hay suficiente man�
    }

    public float ObtenerManaActual()
    {
        return manaActual;
    }
}