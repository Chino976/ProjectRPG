using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    
    // Posicion actual del personaje
    public GameObject rango;
    
    // Se toman todos los datos del tablero
    GameObject tablero;
    
    int posActualX;
    int posActualZ;


    void Start()
    {
        tablero = GameObject.FindWithTag("Tablero");
        
        Vector3 posInicial = new Vector3(0,0,0);
        posInicial = transform.position;

        posActualX = (int)posInicial.x;
        posActualZ = (int)posInicial.z;
        
    }
    
    // Setea la posicion del personaje
    public void SetPos(float x, float z)
    {
        // Cambian los valores de posicion
        transform.position = new Vector3(x, 1.1f, z);
        posActualX = (int)x;
        posActualZ = (int)z;
        tablero.GetComponent<Tablero>().Deseleccionar();
    }
    
    void OnMouseDown() {
        
        if( tablero.GetComponent<Tablero>().pjSeleccionado )
        {
            tablero.GetComponent<Tablero>().Deseleccionar();
            
        }else{
            
            rango.GetComponent<Rango>().Seleccionar(posActualX,posActualZ);
            tablero.GetComponent<Tablero>().tagPersonaje = gameObject.tag;
            tablero.GetComponent<Tablero>().pjSeleccionado = true;
        }
            
    }

}
