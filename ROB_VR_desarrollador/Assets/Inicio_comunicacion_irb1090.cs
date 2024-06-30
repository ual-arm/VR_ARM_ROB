using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inicio_Comunicacion_irb1090 : MonoBehaviour
{

    public Comunicacion_irb1090 Comunicacion_irb1090;
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
        Comunicacion_irb1090.IniciarConexion();
        //Comunicacion_irb140.IniciarConexion_servidor();
    }
}