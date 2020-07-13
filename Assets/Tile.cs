using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


// Este script posee todo lo referente a los "Tiles" o casillas del 
// mapa, ademas de funciones que son utilizadas por el algoritmo BFS
// este script asignara a todas las casillas.
 
public class Tile : MonoBehaviour 
{
    public bool walkable = true; // Indica si la casilla es "caminable" (que el personaje pueda caminar sobre ella o no)
    public bool current = false; // Casill actual (donde esta parado el personaje)
    public bool target = false; // Casilla seleccionado (hacia donde se dirige el personaje)
    public bool selectable = false; // Casilla seleccionable (indica que la casilla puede ser seleccionada para mover el personaje hasta su posicion)
    public bool occuped = false;

    public List<Tile> adjacencyList = new List<Tile>(); // Lista de adyasencia (guarda la casillas adyasentes/vecinas)

    //Needed BFS (breadth first search)
    public bool visited = false; // Casilla visitada
    public Tile parent = null; // Casilla padre (apunta a la casilla desde donde vine)
    public int distance = 0; // Se utiliza para evitar que el mapeo se exeda del tamaño del rango de movimiento del personaje (para que no mapee todo el mundo)


    //For A*
    public float f = 0;
    public float g = 0;
    public float h = 0;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
        if (current)
        {
            GetComponent<Renderer>().material.color = Color.magenta; // Coloca de color magenta la casilla actual
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.green; // Coloca de color verde la casilla seleccionada
        }
        else if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.red; // Coloca de color rojo las casillas que pueden ser seleccionadas
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white; // Coloca de color blanco la casilla por defecto
        }
	}

    // Se resetean todos los valores para actualizarlos por cada frame
    public void Reset()
    {
        adjacencyList.Clear();

        current = false;
        target = false;
        selectable = false;
        occuped = false;

        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }

    // Se buscan los vecinos de la casilla actual
    // jumpHeight es la distancia de altura maxima que se considera caminable, mas se concidera no caminable
    public void FindNeighbors(float jumpHeight, Tile target)
    {
        Reset(); // Se resetean los valores

        CheckTile(Vector3.forward, jumpHeight, target); // Casilla de delante
        CheckTile(-Vector3.forward, jumpHeight, target); // Casilla de detras
        CheckTile(Vector3.right, jumpHeight, target); // Casillad a la dereca
        CheckTile(-Vector3.right, jumpHeight, target); // Casilla a la izquierda
    }

    // Toma la informacion de las casilas indicadas
    public void CheckTile(Vector3 direction, float jumpHeight, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f); // Tamaño de la caja "OverlapBox"

        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents); // Crea unas caja en la direccion indicada y con el tamaño indicado para sensar 
                                                                                                // lo que se encuentra en esa direccion y los guarda en una lista

        foreach (Collider item in colliders) // Por cada item encontrado en esa direccion
        {
            Tile tile = item.GetComponent<Tile>(); // Se obtiene su casilla correspondiente

            if (tile != null && tile.walkable) // Si la casilla no es nula y ademas es "caminable"
            {
                RaycastHit hit; // Se crea un raycast 

                Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1);


                if (hit.collider == null || (tile == target)) // Si no hay nadasobre la casilla vecina o es la casilla selecionada 
                {
                    adjacencyList.Add(tile); // Se añade a la lista de adyasencia
                }
                else if (hit.collider != null)
                {
                    adjacencyList.Add(tile);
                    tile.occuped = true;
                }

            }
        }
    }
}
