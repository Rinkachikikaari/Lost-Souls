using UnityEngine;

public class ControladorActivacion : MonoBehaviour
{
    public int elementosNecesarios = 1; // Cantidad de activadores necesarios
    private int elementosActivados = 0;

    public bool Desactivar;
    public bool Activar;


    public GameObject objetoActivado; // Objeto que se activará (puerta, mecanismo, etc.)

    public void ActivarElemento()
    {
        elementosActivados++;

        if (elementosActivados >= elementosNecesarios)
        {
            ActivarMecanismo();
        }
    }

    public void DesactivarElemento()
    {
        elementosActivados--;

        if (elementosActivados < elementosNecesarios)
        {
            DesactivarMecanismo();
        }
    }

    private void ActivarMecanismo()
    {
        Debug.Log("¡Todos los elementos activados! Activando mecanismo...");
        if (Activar)
        {
            if (objetoActivado != null)
            {
                // Lógica para abrir la puerta o activar el mecanismo
                objetoActivado.SetActive(true); // Ejemplo: activar un objeto
            }
        }
        if (Desactivar)
        {
            if (objetoActivado != null)
            {
                // Lógica para abrir la puerta o activar el mecanismo
                objetoActivado.SetActive(false); // Ejemplo: activar un objeto
            }
        }
    }

    private void DesactivarMecanismo()
    {
        Debug.Log("No todos los elementos están activados. Desactivando mecanismo...");
        if (Activar)
        {
            if (objetoActivado != null)
            {
                // Lógica para abrir la puerta o activar el mecanismo
                objetoActivado.SetActive(false); // Ejemplo: activar un objeto
            }
        }
        if (Desactivar)
        {
            if (objetoActivado != null)
            {
                // Lógica para abrir la puerta o activar el mecanismo
                objetoActivado.SetActive(true); // Ejemplo: activar un objeto
            }
        }
    }
}
