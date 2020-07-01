using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    public Material colorCasilla;
    public int PosCasillaX = 1;
    public int PosCasillaZ = 1;

    GameObject personaje;
    void OnMouseDown() {
        print ("Has clickeado en la casilla (" + PosCasillaX.ToString() + "," + PosCasillaZ.ToString() + ")");
        personaje = GameObject.FindWithTag("Player");

        Vector3 pos = new Vector3(0,0,0);
        pos = transform.position;

        personaje.GetComponent<Personaje>().setPos(pos.x,pos.z);
        personaje.GetComponent<Personaje>().posActualX = PosCasillaX;
        personaje.GetComponent<Personaje>().posActualZ = PosCasillaZ;
    }

    public void PonerColor( Material color_ )
    {
        GetComponent<MeshRenderer> ().material = color_;
        colorCasilla = color_;
    }
}
