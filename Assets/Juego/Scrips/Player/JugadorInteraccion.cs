using UnityEngine;
using UnityEngine.EventSystems;

public class JugadorInteraccion : MonoBehaviour
{
    [Header("Configuración")]
    public float distanciaInteraccion = 1.5f; // Distancia para interactuar con objetos
    public float velocidadArrastre = 2f; // Velocidad al arrastrar
    public float velocidadEmpuje = 1f; // Velocidad al empujar
    public KeyCode teclaArrastrar = KeyCode.E; // Tecla para arrastrar objetos

    private Movimiento movimientoScript;
    private ObjetoInteractivo objetoActual; // Objeto con el que el jugador está interactuando
    public bool agarrando = false; // Indica si el jugador está agarrando un objeto
    private Vector3 moveDirection;
    private Vector3 lastMoveDirection;
    private Vector3 playerMovement;



    private void Start()
    {
        movimientoScript = GetComponent<Movimiento>();
    }
    private void Update()
    {
        if (movimientoScript.movement != Vector3.zero)
        {
            moveDirection = movimientoScript.movement;
        }

        DetectarObjetoInteractivo();

        if (objetoActual != null)
        {
            if (Input.GetKeyDown(teclaArrastrar))
            {
                if (objetoActual != null && objetoActual.esEmpujable)
                {
                    objetoActual.Tomar();
                }
                lastMoveDirection = moveDirection;
                agarrando = true; // Inicia el agarre del objeto
            }

            if (Input.GetKeyUp(teclaArrastrar))
            {
                if (objetoActual != null && objetoActual.esEmpujable)
                {
                    objetoActual.Soltar();
                }
                agarrando = false; // Suelta el objeto
            }
        }

        if(objetoActual != null  && Vector3.Distance(objetoActual.transform.position, this.transform.position) > distanciaInteraccion)
        {
            if (objetoActual.esEmpujable)
            {
                objetoActual.Soltar();
            }
            agarrando = false; // Suelta el objeto
        }
    }

    public void MoveInteraction(Vector3 pMove)
    {
        playerMovement = pMove;
        if (lastMoveDirection.normalized == playerMovement.normalized * -1)
        {
            
            playerMovement *= velocidadArrastre;
            ArrastrarObjeto();
        }
        else if (lastMoveDirection.normalized == playerMovement.normalized)
        {
            playerMovement *= velocidadEmpuje;
            EmpujarObjeto();
        }
        
    }

    private void DetectarObjetoInteractivo()
    {
        // Detectar objetos cercanos usando raycast
        Ray ray = new Ray(transform.position, moveDirection.normalized);
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

        // objetoActual = null; // No hay objeto interactivo cerca
    }

    private void EmpujarObjeto()
    {
        if (objetoActual != null && objetoActual.esEmpujable)
        {
            Vector3 direccionEmpuje = playerMovement ; // Dirección hacia el objeto
            objetoActual.Mover(direccionEmpuje * velocidadEmpuje);
        }
    }

    private void ArrastrarObjeto()
    {
        Debug.Log(playerMovement);
        Debug.Log((objetoActual != null) + " " + objetoActual.esArrastrable);
        if (objetoActual != null && objetoActual.esArrastrable)
        {
            Vector3 direccionArrastre = (playerMovement); // Dirección hacia atrás
            
            objetoActual.Mover(direccionArrastre * velocidadArrastre);
        }
    }

    public float PesoObjeto()
    {
        return objetoActual.peso;
    }

    // Visualizar el raycast en la escena con Gizmos
    private void OnDrawGizmos()
    {


        // Ray de interacciones
        Gizmos.color = Color.red; // Color del raycast
        Gizmos.DrawRay(transform.position, moveDirection.normalized * distanciaInteraccion);

        // Opcionalmente, dibujar un círculo en la posición del objeto interactivo
        if (objetoActual != null)
        {
            Gizmos.color = Color.green; // Color para el objeto interactivo
            Gizmos.DrawSphere(objetoActual.transform.position, 0.5f); // Cambiar el radio del círculo si es necesario
        }
    }
}
