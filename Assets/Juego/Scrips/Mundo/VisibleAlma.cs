using UnityEngine;

public class VisibleAlma : MonoBehaviour
{
    public GameObject objeto;
    private void OnEnable()
    {
        CambioAlmas.OnCambioAlma += ManejarCambioAlma; // Suscribirse al evento
    }

    private void OnDisable()
    {
        CambioAlmas.OnCambioAlma -= ManejarCambioAlma; // Desuscribirse del evento
    }

    private void ManejarCambioAlma(bool esAlmaActual)
    {
        objeto.SetActive(!esAlmaActual); // Activa este objeto solo si no es el alma actual
        Debug.Log($"Cambio detectado. VisibleAlma activo: {!esAlmaActual}");
    }
}
