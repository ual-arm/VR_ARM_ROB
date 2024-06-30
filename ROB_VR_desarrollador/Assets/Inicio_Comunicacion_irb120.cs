using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inicio_Comunicacion_irb120 : MonoBehaviour
{

    public Comunicacion_irb120 Comunicacion_irb120;
    // Start is called before the first frame update
    void Start()
    {
        //Comunicacion = GetComponent<Comunicacion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            IniciarComunicacion();
        }
    }

    void IniciarComunicacion()
    {
        Comunicacion_irb120.IniciarConexion();
    }
}

