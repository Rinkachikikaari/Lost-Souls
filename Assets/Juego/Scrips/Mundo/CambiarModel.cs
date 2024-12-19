using System;
using UnityEngine;

public class CambiarModel : MonoBehaviour
{
    private bool activado = false;
    private bool jugadorCerca = false; // Indica si el jugador está cerca
    public string POA;
    public bool Interactivo;

    public GameObject Abierto;
    public GameObject Cerrado;



    private void Update()
    {
        if (jugadorCerca && Interactivo && Input.GetKeyDown(KeyCode.E))
        {
            if (!activado)
            {
                activado = true;
                Abierto.SetActive(true);
                Cerrado.SetActive(false);
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(POA))
        {
            jugadorCerca = false;
            Debug.Log("Jugador salió del activador: " + gameObject.name);
        }
    }
}
