using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    GameObject tablero;
    public List<GameObject> casillas = new List<GameObject>();
    
    public int kRango = 2;
    public int posActualX;
    public int posActualZ;
    bool pjSeleccionado = false;

    public void setPos(float x, float z)
    {
        transform.position = new Vector3(x, 1.1f, z);
        posActualX = (int)x;
        posActualZ = (int)z;
        pjSeleccionado = false;
    }

    void OnMouseDown() {
        
        pjSeleccionado = true;
        
        tablero = GameObject.FindWithTag("Tablero");
        int filas = tablero.GetComponent<CrearCasilla>().filas;
        int columnas = tablero.GetComponent<CrearCasilla>().columnas;
        int numCasillas = filas*columnas;
        
        List<GameObject> todasCasillas = tablero.GetComponent<CrearCasilla>().casillas;
        
        print("Pos personaje (" + posActualX.ToString() + "," + posActualZ.ToString() + ")");

        //casillas = new List<GameObject>();
        
        for( int i = posActualZ-kRango; i < 1+posActualZ+kRango; i++ )
        {
            for( int j = posActualX-kRango; j < 1+posActualX+kRango; j++ )
            {
                for( int k = 0; k < numCasillas; k++)
                {
                    int xPos = todasCasillas[k].GetComponent<Casilla>().posCasillaX;
                    int zPos = todasCasillas[k].GetComponent<Casilla>().posCasillaZ;
                    
                    if( xPos == j && zPos == i )
                    {
                        todasCasillas[k].GetComponent<Casilla>().Seleccionar(true);
                        casillas.Add(todasCasillas[k]);
                    }
                }
            }
        }
        
    }
    
    void Update()
    {    
        if(!pjSeleccionado && casillas.Count > 0)
        {
            print("personaje deseleccionado");
            foreach(GameObject i in casillas){
                i.GetComponent<Casilla>().Seleccionar(false);
            }
            casillas.Clear();
        }
    }
}
