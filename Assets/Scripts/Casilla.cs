using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    public Material colorCasilla;
    public Material rangoColor;
    
    public Vector3 pos;

    GameObject personaje;
    GameObject tablero;
    
    public bool casillaActiva = false;
    
    void Start()
    {
        GetComponent<MeshRenderer> ().material = colorCasilla;
        pos = transform.position;
        tablero = GameObject.FindWithTag("Tablero");
    }

    void OnMouseDown() {

        print ("Has clickeado en la casilla (" + pos.x + "," + pos.z + ") - Activada: " + casillaActiva);
        if(casillaActiva)
        {
            personaje = GameObject.FindWithTag( tablero.GetComponent<Tablero>().tagPersonaje ); // Se busca el personaje en la escena con e tag indicado
            personaje.GetComponent<Personaje>().SetPos(pos.x, pos.z); // se envia la posicion para realizar el movimiento
        }

    }

    public void Validar( bool estado )
    {
        if(estado)
        {
            GetComponent<MeshRenderer> ().material = rangoColor; //Se setea el color de seleccion
        }else{
            GetComponent<MeshRenderer> ().material = colorCasilla;
        }
        casillaActiva = estado;
    }


    public void OnTriggerEnter( Collider collider )
    {
        if (collider.gameObject.tag == "Rango" && !casillaActiva )
        {
            tablero.GetComponent<Tablero>().casillasRango.Add( gameObject );
            Validar( true );
        }
    }

}
