using UnityEngine;
using System.Collections.Generic;

public class GeneradorMazmorra : MonoBehaviour
{
    [System.Serializable]
    public class HabitacionPrefab
    {
        public GameObject prefab;
        public Vector2Int tamaño; // Tamaño en celdas, por ejemplo, (2,2) para una habitación de 2x2
    }

    public List<HabitacionPrefab> prefabsHabitaciones; // Lista de prefabs de habitaciones de varios tamaños
    public int numeroHabitaciones = 5;
    public int anchuraMazmorra = 20; // Tamaño de la cuadrícula de la mazmorra
    public int alturaMazmorra = 20;

    private bool[,] celdasOcupadas;
    private List<Vector2Int> direcciones = new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    private void Start()
    {
        celdasOcupadas = new bool[anchuraMazmorra, alturaMazmorra];
        GenerarMazmorra();
    }

    private void GenerarMazmorra()
    {
        // Empezar la mazmorra en una posición inicial (centro de la cuadrícula)
        Vector2Int posicionInicial = new Vector2Int(anchuraMazmorra / 2, alturaMazmorra / 2);
        GenerarHabitacionAleatoria(posicionInicial);

        int habitacionesGeneradas = 1;

        while (habitacionesGeneradas < numeroHabitaciones)
        {
            Vector2Int posicionDisponible = ObtenerPosicionAdyacente();

            if (posicionDisponible != Vector2Int.one * -1)
            {
                GenerarHabitacionAleatoria(posicionDisponible);
                habitacionesGeneradas++;
            }
        }
    }

    private void GenerarHabitacionAleatoria(Vector2Int posicion)
    {
        HabitacionPrefab habitacionSeleccionada = prefabsHabitaciones[Random.Range(0, prefabsHabitaciones.Count)];

        // Verificar si hay espacio para colocar la habitación seleccionada
        if (EspacioDisponible(posicion, habitacionSeleccionada.tamaño))
        {
            Vector3 posicionMundo = new Vector3(posicion.x * 10, 0, posicion.y * 10);
            GameObject habitacion = Instantiate(habitacionSeleccionada.prefab, posicionMundo, Quaternion.identity);

            // Marcar las celdas ocupadas por esta habitación
            for (int x = 0; x < habitacionSeleccionada.tamaño.x; x++)
            {
                for (int y = 0; y < habitacionSeleccionada.tamaño.y; y++)
                {
                    celdasOcupadas[posicion.x + x, posicion.y + y] = true;
                }
            }
        }
    }

    private Vector2Int ObtenerPosicionAdyacente()
    {
        List<Vector2Int> posiblesPosiciones = new List<Vector2Int>();

        // Buscar posiciones adyacentes a habitaciones existentes
        for (int x = 0; x < anchuraMazmorra; x++)
        {
            for (int y = 0; y < alturaMazmorra; y++)
            {
                if (celdasOcupadas[x, y]) // Si hay una habitación aquí
                {
                    foreach (var direccion in direcciones)
                    {
                        Vector2Int posicionAdyacente = new Vector2Int(x, y) + direccion;

                        if (EsPosicionValida(posicionAdyacente) && EspacioDisponible(posicionAdyacente, prefabsHabitaciones[0].tamaño)) // tamaño inicial
                        {
                            posiblesPosiciones.Add(posicionAdyacente);
                        }
                    }
                }
            }
        }

        return posiblesPosiciones.Count > 0 ? posiblesPosiciones[Random.Range(0, posiblesPosiciones.Count)] : Vector2Int.one * -1;
    }

    private bool EspacioDisponible(Vector2Int posicion, Vector2Int tamaño)
    {
        // Verificar si hay espacio suficiente en la cuadrícula para colocar la habitación
        for (int x = 0; x < tamaño.x; x++)
        {
            for (int y = 0; y < tamaño.y; y++)
            {
                Vector2Int pos = new Vector2Int(posicion.x + x, posicion.y + y);

                if (!EsPosicionValida(pos) || celdasOcupadas[pos.x, pos.y])
                {
                    return false; // Espacio ocupado o fuera de los límites
                }
            }
        }
        return true;
    }

    private bool EsPosicionValida(Vector2Int posicion)
    {
        return posicion.x >= 0 && posicion.x < anchuraMazmorra && posicion.y >= 0 && posicion.y < alturaMazmorra;
    }
}
