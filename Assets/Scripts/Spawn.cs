using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public int cantPersonajes;
    public GameObject[] personaje;
    public GameObject[] casillaSpawn;
    
        // Start is called before the first frame update
    void Start()
    {
        // Vector temporal de posicion    
        Vector3 posSpawn = new Vector3(0,0,0);
        
        for(int i = 0; i < cantPersonajes; i++ )
        {
            posSpawn = casillaSpawn[i].transform.position; // se toma la posicion de la casilla
            
            Instantiate(personaje[i], new Vector3(posSpawn.x,1,posSpawn.z), Quaternion.identity); // se crea el personaje en la posicion indicada
            
        }
        
    }

}
