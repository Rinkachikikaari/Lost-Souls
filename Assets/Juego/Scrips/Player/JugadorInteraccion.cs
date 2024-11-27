using UnityEngine;

public class JugadorInteraccion : MonoBehaviour
{
    [Header("Configuración")]
    public float distanciaInteraccion = 1.5f; // Distancia para interactuar con objetos
    public float velocidadArrastre = 2f; // Velocidad al arrastrar
    public float velocidadEmpuje = 1f; // Velocidad al empujar
    public KeyCode teclaArrastrar = KeyCode.E; // Tecla para arrastrar objetos

    private ObjetoInteractivo objetoActual; // Objeto con el que el jugador está interactuando
    private bool agarrando = false; // Indica si el jugador está agarrando un objeto

    private void Update()
    {
        DetectarObjetoInteractivo();

        if (objetoActual != null)
        {
            if (Input.GetKeyDown(teclaArrastrar))
            {
                agarrando = true; // Inicia el agarre del objeto
            }

            if (Input.GetKeyUp(teclaArrastrar))
            {
                agarrando = false; // Suelta el objeto
            }

            if (agarrando)
            {
                ArrastrarObjeto();
            }
            else
            {
                EmpujarObjeto();
            }
        }
    }

    private void DetectarObjetoInteractivo()
    {
        // Detectar objetos cercanos usando raycast
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaInteraccion))
        {
            ObjetoInteractivo objeto = hit.collider.GetComponent<ObjetoInteractivo>();
            if (objeto != null)
            {
                objetoActual = objeto;
                return;
            }
        }

        objetoActual = null; // No hay objeto interactivo cerca
    }

    private void EmpujarObjeto()
    {
        if (objetoActual != null && objetoActual.esEmpujable)
        {
            Vector3 direccionEmpuje = transform.forward; // Dirección hacia el objeto
            objetoActual.Mover(direccionEmpuje * velocidadEmpuje);
        }
    }

    private void ArrastrarObjeto()
    {
        if (objetoActual != null && objetoActual.esArrastrable)
        {
            Vector3 direccionArrastre = (objetoActual.transform.position - transform.position).normalized; // Dirección hacia atrás
            objetoActual.Mover(-direccionArrastre * velocidadArrastre);
        }
    }
}
