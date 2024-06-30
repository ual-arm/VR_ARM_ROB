using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlComunicaciones : MonoBehaviour
{

    public Comunicacion Comunicacion;
    // Start is called before the first frame update
    void Start()
    {
        //Comunicacion = GetComponent<Comunicacion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            IniciarComunicacion();
        }
    }

    void IniciarComunicacion()
    {
        Comunicacion.IniciarConexion();
    }
}
