using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_com_irb140 : MonoBehaviour
{
    public GameObject control;
    public Comunicacion_irb140 Comunicacion_irb140;
    private bool x;
    // Start is called before the first frame update
    void OnEnable()
    {
        x = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (control.activeSelf==false)
        {

            if (x == false)
            {
                //Debug.Log("Cerrando conexion");
                Comunicacion_irb140.FinalizarConexion();
                x = true;
            }

        }
        else
        {
            x = false;
        }
    }
}
