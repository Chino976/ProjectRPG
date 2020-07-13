using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Este script hereda todas las funciones de "TacticsMove" y 
// agrega funciones especificas del movimiento del personaje. 
public class PlayerMove : TacticsMove 
{

	// Use this for initialization
	void Start () 
	{
        Init();
	}
	
	// Update is called once per frame
	void Update () 
	{
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            return;
        }

        if (!moving) // Si el personaje no se esta moviendo
        {
            FindSelectableTiles(); // Se buscan las casillas seleccionable
            CheckMouse(); 
        }
        else
        {
            Move();
        }
	}

    // Esta funcion verifica si se a cliqueado alguna casilla
    void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0)) // Si se clickea el boton izquierdo del mouse
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Se crea un rayo desde la posicion de la camara en direccion al puntero del mouse

            RaycastHit hit; // Variable de salida para el rayo (se guardan los datos del object que haya golpeado)
            if (Physics.Raycast(ray, out hit)) // si el rayo golpea algo
            {
                if (hit.collider.tag == "Tile") // y ese algo es un GameObject con tag de "Tile"
                {
                    Tile t = hit.collider.GetComponent<Tile>(); // Se extrae el componente "Tile" (script)

                    if (t.selectable) // Si la casilla es una casilla seleccionable (dentro del rango de movimiento)
                    {
                        MoveToTile(t); // Se llama a la funcion de movimiento
                    }
                }
            }
        }
    }
}
