using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    GameObject tablero;
    GameObject[] Rango;
    
    public int kRango;
    public int posActualX;
    public int posActualZ;

    public void setPos(float x, float z)
    {
        transform.position = new Vector3(x, 1.1f, z);
    }

    void OnMouseDown() {
        tablero = GameObject.FindWithTag("Tablero");
        int sizeTablero = tablero.GetComponent<CrearCasilla>().Filas*tablero.GetComponent<CrearCasilla>().Columnas;
        print("Click en personaje" + sizeTablero.ToString());

        Rango = new GameObject[kRango];

        for( int i = 0; i < sizeTablero; i++ )
        {

        }
    }
}
