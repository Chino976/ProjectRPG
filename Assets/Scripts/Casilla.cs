using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    public Material colorCasilla;
    public Material rangoColor;
    public int posCasillaX = 1;
    public int posCasillaZ = 1;
    bool activa = false;
    bool colorAplicado = true;

    GameObject personaje;
    
    void OnMouseDown() {
        
        print ("Has clickeado en la casilla (" + posCasillaX.ToString() + "," + posCasillaZ.ToString() + ") - Activada: " + activa);
        
        if(activa)
        {
            personaje = GameObject.FindWithTag("Player");
    
            Vector3 pos = new Vector3(0,0,0);
            pos = transform.position;
            
            personaje.GetComponent<Personaje>().setPos(pos.x,pos.z);
            
        }
        
    }
    
    public void Seleccionar( bool activar )
    {
        activa = activar;
        colorAplicado = false;
    }

    public void PonerColor( Material color_ )
    {
        GetComponent<MeshRenderer> ().material = color_;
        colorCasilla = color_;
    }
    
    void Update()
    {
        if(activa && !colorAplicado)
        {
            print("color de seleccion");
            GetComponent<MeshRenderer> ().material = rangoColor;
            colorAplicado = true;
            
        }else if (!activa && !colorAplicado){
            print("color sin seleccion");
            GetComponent<MeshRenderer> ().material = colorCasilla;
            colorAplicado = true;
        }
        
    }
}
