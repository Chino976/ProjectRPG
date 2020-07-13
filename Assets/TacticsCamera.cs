using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Este script hace girar la camara en torno al mapa, se pueden 
// agregar mas funciones como por ej. mover la camara/subirlas etc.
public class TacticsCamera : MonoBehaviour 
{
    public void RotateLeft()
    {
        transform.Rotate(Vector3.up, 90, Space.Self);
    }

    public void RotateRight()
    {
        transform.Rotate(Vector3.up, -90, Space.Self);
    }
}
