using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Estet script es un script padre, contendra todas las funciones base 
// referentes al movimiento de los personajes sobre el tablero
// luego de el decenderan scripts que hereden estas funciones, y añadan
// otras nuevas dependiendo de la funcion de cada personaje, este script
// no sera añadido a ningun GameObject en pantalla sino que sera usado 
// atravez de sus script hijos.

public class TacticsMove : MonoBehaviour
{
    public bool turn = false;

    List<Tile> selectableTiles = new List<Tile>(); // Lista de las casillas habilitadas para realizar el movimiento (rango de movimiento)
    GameObject[] tiles; // Lista que contendra todas las casillas del mapa para acceder a ellas mas facilmente

    Stack<Tile> path = new Stack<Tile>(); // Lista de casillas que finalmente conforman el camino origen-destino (camino final a seguir)
    Tile currentTile; // Indica en que casilla esta parado el personaje actualmente

    public bool moving = false; // Esto indica si el personaje esta en movimiento
    public int move = 5; // Rango de movimiento
    public float jumpHeight = 2; // Altura del salto
    public float moveSpeed = 2; // Velocidad de movimiento
    public float jumpVelocity = 4.5f; // Velocidad del salto

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0; // Sirve para saber la distancia desde la casilla hasta el centro del personaje

    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingEdge = false;
    Vector3 jumpTarget;

    public Tile actualTargetTile;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile"); // Se obtienen y se guardan todas las casillas en el mapa

        halfHeight = GetComponent<Collider>().bounds.extents.y; // Se ontienene la distancia

        TurnManager.AddUnit(this);
    }

    // Se utiliza para obtener la casilla actual
    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject); // Se envia el GameObject relacionado a este script para extraer su casilla
        currentTile.current = true; // Se indica la casilla como casilla actual (Se pone de color magenta)
    }

    // Esta funcion obtiene la casilla perteneciente al GameObject que pasamos por parametro
    public Tile GetTargetTile(GameObject target) 
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1)) //Si el raycas golpea un collider debajo del GameObject
        {
            tile = hit.collider.GetComponent<Tile>(); // Extraemos el componente "Tile" correspondiente
        }

        return tile; // Retornamos el "Tile" obtenido  
    }

    public void ComputeAdjacencyLists(float jumpHeight, Tile target)
    {
        //tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles) // Se recorren todas las casillas del mapa 
        {
            Tile t = tile.GetComponent<Tile>(); // Se obtiene el componente "Tile" de cada casilla
            t.FindNeighbors(jumpHeight, target); // Y se buscan sus vecinos
        }
    }

    // El error de que no te deja seleccionar las casillas detras del enemigo es porqie 
    // para llegar a dichas casillas hay que esquivar al otro npc lo que causa que las 
    // distancias a esas casillas sean mayores y no entran dentro del if de t.distance < move.
    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists(jumpHeight, null); // Se actualiza la lista de adyacencia
        GetCurrentTile(); // Se obtiene la casilla actual

        Queue<Tile> process = new Queue<Tile>(); // Se crea una cola de casillas procesadas

        process.Enqueue(currentTile); // Se encola la casilla actual o inicial

        currentTile.visited = true; // Se setea la casilla actual como visitada
        //currentTile.parent = ??  leave as null 

        while (process.Count > 0) // Mientras la cola no este vacia
        {
            Tile t = process.Dequeue(); // Sacar de la cola el ultimo Tile

            if(!t.occuped)
            {
                selectableTiles.Add(t); // Se añade a la lista de casillas seleccionable
                t.selectable = true; // Y se setea la casilla como selecionable (La casilla se pinta de rojo)
            }

            if (t.distance < move) // Si la distancia de la casilla es menor al rango de movimiento (Esto limita el tamaño de la busqueda y ahorra recursos)
            {
                foreach (Tile tile in t.adjacencyList) // Para cada Tile/casilla adyasente/vecina a la casilla "t" 
                {
                    if (!tile.visited) // Y si dicha casilla no fue visitada con anterioridad
                    {
                        tile.parent = t; // Se setea el padre de la casilla vecina y se le asigna t
                        tile.visited = true; // Se setea la casilla como visitada
                        tile.distance = 1 + t.distance; // A la distancia de la casilla vecina se le suma la distancia que acarrea el padre
                        process.Enqueue(tile); // Se encola la casilla
                    }
                }
            }
        }
    }

    // Funcion de movimiento de personaje
    public void MoveToTile(Tile tile)
    {
        path.Clear(); // Se limpia cualquier cosa que pudiera estar en el path
        tile.target = true; // Se setea la casilla como seleccionada (Verde - destino)
        moving = true; // Se informa que el personaje esta en movimiento

        Tile next = tile; // Se setea el inicio de la busqueda
        while (next != null) // Mientras no se recorran todas las casillas hasta el origen
        {
            path.Push(next); // Se guarda el camino
            next = next.parent; // Y se pasa a la casilla padre
        }
    }

    public void Move()
    {
        if (path.Count > 0) // Si la cantidad de casillas en el Path sea mayor a 0
        {
            Tile t = path.Peek(); // Leemos lo que esta en la parte superior del stack path (no la extraemos)
            Vector3 target = t.transform.position; // Tomamos la posicion correspondiente el Tile encontrado

            //Calculate the unit's position on top of the target tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y; // Calcula la posición de la unidad en la parte superior de la casilla de destino

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                bool jump = transform.position.y != target.y;

                if (jump)
                {
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizotalVelocity();
                }

                //Locomotion
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //Tile center reached
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();
            moving = false;

            TurnManager.EndTurn();
        }
    }


    // Esto cambia las casillas que han sido marcadas como seleccionable a como estaban por defecto
    protected void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizotalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    void Jump(Vector3 target)
    {
        if (fallingDown)
        {
            FallDownward(target);
        }
        else if (jumpingUp)
        {
            JumpUpward(target);
        }
        else if (movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }

    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;
        target.y = transform.position.y;

        CalculateHeading(target);

        if (transform.position.y > targetY)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;

            jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        else
        {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            velocity = heading * moveSpeed / 3.0f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    }

    void FallDownward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y <= target.y)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }

    void JumpUpward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizotalVelocity();
        }
        else
        {
            movingEdge = false;
            fallingDown = true;

            velocity /= 5.0f;
            velocity.y = 1.5f;
        }
    }

    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= move)
        {
            return t.parent;
        }

        Tile endTile = null;
        for (int i = 0; i <= move; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;
    }

    protected void FindPath(Tile target)
    {
        ComputeAdjacencyLists(jumpHeight, target);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        //currentTile.parent = ??
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);

            closedList.Add(t);

            if (t == target)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }

            foreach (Tile tile in t.adjacencyList)
            {
                if (closedList.Contains(tile))
                {
                    //Do nothing, already processed
                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }
        }

        //todo - what do you do if there is no path to the target tile?
        Debug.Log("Path not found");
    }

    public void BeginTurn()
    {
        turn = true;
    }

    public void EndTurn()
    {
        turn = false;
    }
}
