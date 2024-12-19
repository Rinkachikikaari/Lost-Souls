using UnityEngine;

public class Activador : MonoBehaviour
{
    public string objetoRequeridoTag; // Tag del objeto que debe interactuar (por ejemplo, "Roca")
    public ControladorActivacion controlador; // Referencia al controlador
    private bool activado = false;
    public bool Interactivo;
    public string POA;

    private bool jugadorCerca = false; // Indica si el jugador est� cerca

    private void Update()
    {
        if (jugadorCerca && Interactivo && Input.GetKeyDown(KeyCode.E))
        {
            if (!activado)
            {
                activado = true;
                controlador.ActivarElemento(); // Enviar se�al al controlador
                Debug.Log("Activador activado: " + gameObject.name);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(POA))
        {
            jugadorCerca = true;
            Debug.Log("Jugador cerca del activador: " + gameObject.name);
        }

        if (!Interactivo && other.CompareTag(objetoRequeridoTag) && !activado)
        {
            activado = true;
            controlador.ActivarElemento(); // Enviar se�al al controlador
            Debug.Log("Activador activado: " + gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(POA))
        {
            jugadorCerca = false;
            Debug.Log("Jugador sali� del activador: " + gameObject.name);
        }

        if (!Interactivo && other.CompareTag(objetoRequeridoTag) && activado)
        {
            activado = false;
            controlador.DesactivarElemento(); // Enviar se�al de desactivaci�n al controlador
            Debug.Log("Activador desactivado: " + gameObject.name);
        }
    }
}
