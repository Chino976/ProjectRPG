using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablero : MonoBehaviour
{
    //public GameObject[] listaCasillas;
    public List <GameObject> casillasRango ;
    public string tagPersonaje;
    public bool pjSeleccionado = false;
    // Start is called before the first frame update
    void Start()
    {
        casillasRango = new List <GameObject>();
    }
    
    public void Deseleccionar()
    {
        foreach(GameObject casilla in casillasRango)
        {
            casilla.GetComponent<Casilla>().Validar(false);
        }
        
        casillasRango.Clear();
        pjSeleccionado = false;
    }
    
}
