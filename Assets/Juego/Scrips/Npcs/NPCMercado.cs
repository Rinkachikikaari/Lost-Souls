using System.Collections.Generic;
using UnityEngine;

public class NPCMercado : MonoBehaviour
{
    [Header("Configuraci�n de la Tienda")]
    public string mensajeBienvenida = "Bienvenido a la tienda, busca lo que deseas en el mostrador.";
    public GameObject panelDialogo; // Panel para mostrar el mensaje inicial
    public UnityEngine.UI.Text textoDialogo; // Texto del mensaje inicial

    [Header("Configuraci�n del Mostrador")]
    public List<GameObject> objetosMostrador; // Lista de objetos en el mostrador
    public List<Transform> puntosColocacion; // Puntos de colocaci�n en el mostrador
    public List<GameObject> inventario; // Inventario del vendedor
    public float tiempoRevision = 300f; // 5 minutos en segundos


    private bool cercaDelNPC = false;

    private void Start()
    {
        if (panelDialogo != null)
        {
            panelDialogo.SetActive(false); // El panel de di�logo est� oculto al inicio
        }
        // Configurar mostrador al inicio
        ActualizarMostrador();

        // Revisar y actualizar mostrador peri�dicamente
        InvokeRepeating("RevisarMostrador", tiempoRevision, tiempoRevision);
    }

    private void Update()
    {
        if (cercaDelNPC && Input.GetKeyDown(KeyCode.E))
        {
            MostrarMensajeBienvenida();
        }
    }

    private void MostrarMensajeBienvenida()
    {
        if (panelDialogo != null && textoDialogo != null)
        {
            panelDialogo.SetActive(true);
            textoDialogo.text = mensajeBienvenida;

            // Ocultar el mensaje despu�s de unos segundos
            Invoke("OcultarMensaje", 3f);
        }
    }

    private void OcultarMensaje()
    {
        if (panelDialogo != null)
        {
            panelDialogo.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cercaDelNPC = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cercaDelNPC = false;
        }
    }

    private void RevisarMostrador()
    {
        for (int i = 0; i < puntosColocacion.Count; i++)
        {
            // Si no hay un objeto en el mostrador o el actual est� inactivo
            if (i >= objetosMostrador.Count || objetosMostrador[i] == null || !objetosMostrador[i].activeSelf)
            {
                SacarItemDelInventario(i);
            }
        }
    }

    private void SacarItemDelInventario(int index)
    {
        if (inventario.Count > 0) // Verifica que haya �tems disponibles en el inventario
        {
            // Selecciona el primer objeto del inventario
            GameObject itemSeleccionado = inventario[0];

            // Instancia el objeto en el punto correspondiente
            GameObject nuevoObjeto = Instantiate(itemSeleccionado, puntosColocacion[index].position, puntosColocacion[index].rotation);
            nuevoObjeto.name = itemSeleccionado.name; // Asegura que conserve el nombre del prefab

            // Agrega el objeto a la lista de mostrador
            if (index < objetosMostrador.Count)
            {
                objetosMostrador[index] = nuevoObjeto; // Reemplaza el objeto en el mostrador
            }
            else
            {
                objetosMostrador.Add(nuevoObjeto); // A�ade el objeto al mostrador
            }

            // Elimina el objeto del inventario
            inventario.RemoveAt(0);

            Debug.Log($"Se ha colocado {nuevoObjeto.name} en el mostrador y eliminado del inventario.");
        }
        else
        {
            Debug.LogWarning("No hay m�s �tems en el inventario para colocar en el mostrador.");
        }
    }

    private void ActualizarMostrador()
    {
        for (int i = 0; i < puntosColocacion.Count; i++)
        {
            SacarItemDelInventario(i); // Coloca un �tem en cada punto
        }
    }
}
