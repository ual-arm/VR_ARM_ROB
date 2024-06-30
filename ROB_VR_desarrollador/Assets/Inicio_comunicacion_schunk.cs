using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inicio_Comunicacion_schunk : MonoBehaviour
{

    public Comunicacion_schunk Comunicacion_schunk;
    // Start is called before the first frame update

    void OnEnable()
    {
        //if (Input.GetKeyUp(KeyCode.V))
        //{
        IniciarComunicacion();
        //}
    }

    // Update is called once per frame


    void IniciarComunicacion()
    {
        Comunicacion_schunk.IniciarConexion();
        //Comunicacion_irb140.IniciarConexion_servidor();
    }
}
