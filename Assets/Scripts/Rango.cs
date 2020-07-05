using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rango : MonoBehaviour
{
    public int kRango;
    public bool rangoActivo = false;

    int posX;
    int posZ;
    
    
    void Start()
    {
        Vector3 pos = transform.position;
        
        posX = (int)pos.x;
        posZ = (int)pos.z;
        
        Vector3 escalado = new Vector3( kRango, 1, kRango);
        transform.localScale = transform.localScale + escalado;
        
    }
    
    public void Seleccionar( int x, int z)
    {
        posX = x;
        posZ = z;
        
        transform.position = new Vector3(posX,-0.5f,posZ);
    }
    
    void OnMouseOver()
    {
        transform.position = new Vector3(posX,-3,posZ);
    }

}
