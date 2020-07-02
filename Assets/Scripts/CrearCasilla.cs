using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearCasilla : MonoBehaviour
{
    public GameObject casillaPrefab;
    public GameObject personaje;
    
    public List<GameObject> casillas;
    
    public int filas;
    public int columnas;

    public Material blanco;
    public Material negro;

    public void Crear()
    {
        Instantiate(personaje);

        casillas = new List<GameObject>();
        
        for( int i = 0; i < columnas; i++ )
        {
            for( int j = 0; j < filas; j++ )
            {
                GameObject casillaTemp = Instantiate(casillaPrefab,new Vector3(j,0,i),Quaternion.identity);

                if( (i+j) % 2 == 0)
                {
                    casillaTemp.GetComponent<Casilla>().PonerColor( negro );
                }else{
                    casillaTemp.GetComponent<Casilla>().PonerColor( blanco );
                }
                casillaTemp.GetComponent<Casilla>().posCasillaX = j;
                casillaTemp.GetComponent<Casilla>().posCasillaZ = i;
                
                casillas.Add(casillaTemp);

            }

        }
    }
}
