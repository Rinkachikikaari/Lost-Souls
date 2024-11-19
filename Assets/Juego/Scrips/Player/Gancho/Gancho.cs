using System.Collections;
using UnityEngine;

public class Gancho : MonoBehaviour
{
    public GameObject prefabGancho;           // Prefab del objeto gancho
    public Transform puntoDisparo;           // Punto desde el cual se dispara el gancho
    public float ganchoVelocidad = 15f;

    private Movimiento movimientoScript;
    private Ataque ataqueScript;
    private DisparoFlecha arcoScript;
    private bool ganchoActivo = false;
    private Vector3 ultimaDireccion;

    private void Start()
    {
        movimientoScript = GetComponent<Movimiento>();
        ataqueScript = GetComponent<Ataque>();
        arcoScript = GetComponent<DisparoFlecha>();
    }

    private void Update()
    {
        if (movimientoScript.movement != Vector3.zero)
        {
            ultimaDireccion = movimientoScript.movement;
        }

        if (Input.GetKeyDown(KeyCode.Q) && !ganchoActivo)
        {
            LanzarGancho();
        }
    }

    private void LanzarGancho()
    {
        if (prefabGancho != null && ultimaDireccion != Vector3.zero)
        {
            GameObject ganchoInstancia = Instantiate(prefabGancho, puntoDisparo.position, Quaternion.identity);
            if (ganchoInstancia != null)
            {
                GanchoObjeto ganchoScript = ganchoInstancia.GetComponent<GanchoObjeto>();
                ganchoScript.ConfigurarGancho(ultimaDireccion, transform, this);

                movimientoScript.enabled = false;
                ataqueScript.enabled = false;
                arcoScript.enabled = false;
                ganchoActivo = true;
            }
        }
    }

    public void AtraerObjetivo(Transform objetivo)
    {
        Enemy enemigo = objetivo.GetComponent<Enemy>();
        if (enemigo != null)
        {
            Debug.Log($"{objetivo.name} es un enemigo y está siendo atraído.");
            StartCoroutine(AtraerObjetoHaciaJugador(objetivo));
            return;
        }

        GanchoInteractivo interactivo = objetivo.GetComponent<GanchoInteractivo>();
        if (interactivo != null)
        {
            Debug.Log($"{objetivo.name} es un objeto interactivo y está siendo atraído.");
            StartCoroutine(AtraerObjetoHaciaJugador(objetivo));
            return;
        }
    }

    public void MoverHaciaSuperficie(Vector3 posicionSuperficie)
    {
        Debug.Log("El jugador se está moviendo hacia la superficie.");
        StartCoroutine(MoverJugadorHaciaSuperficie(posicionSuperficie));
    }

    private IEnumerator AtraerObjetoHaciaJugador(Transform objetivo)
    {
        while (Vector3.Distance(objetivo.position, transform.position) > 1f)
        {
            objetivo.position = Vector3.MoveTowards(objetivo.position, transform.position, ganchoVelocidad * Time.deltaTime);
            yield return null;
        }
        FinGancho();
    }

    private IEnumerator MoverJugadorHaciaSuperficie(Vector3 posicionObjetivo)
    {
        while (Vector3.Distance(transform.position, posicionObjetivo) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, ganchoVelocidad * Time.deltaTime);
            yield return null;
        }
        FinGancho();
    }

    public void FinGancho()
    {
        ganchoActivo = false;
        movimientoScript.enabled = true;
        ataqueScript.enabled = true;
        arcoScript.enabled = true;
        Debug.Log("El gancho ha finalizado su acción.");
    }
}
