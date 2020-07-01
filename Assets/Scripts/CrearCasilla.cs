using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearCasilla : MonoBehaviour
{
    public GameObject CasillaPrefab;

    public GameObject Personaje;
    public int Filas;
    public int Columnas;

    public Material Blanco;
    public Material Negro;

    public void Crear()
    {
        Instantiate(Personaje);


        for( int i = 0; i < Columnas; i++ )
        {
            for( int j = 0; j < Filas; j++ )
            {
                GameObject casillaTemp = Instantiate(CasillaPrefab,new Vector3(j,0,i),Quaternion.identity);

                if( (i+j) % 2 == 0)
                {
                    casillaTemp.GetComponent<Casilla>().PonerColor( Negro );
                }else{
                    casillaTemp.GetComponent<Casilla>().PonerColor( Blanco );
                }
                casillaTemp.GetComponent<Casilla>().PosCasillaX = j;
                casillaTemp.GetComponent<Casilla>().PosCasillaZ = i;

            }



        }
    }
}
