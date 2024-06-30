using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_com_irb1090 : MonoBehaviour
{
    public GameObject control;
    public Comunicacion_irb1090 Comunicacion_irb1090;
    private bool x;
    // Start is called before the first frame update
    void OnEnable()
    {
        x = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (control.activeSelf == false)
        {

            if (x == false)
            {
                //Debug.Log("Cerrando conexion");
                Comunicacion_irb1090.FinalizarConexion();
                x = true;
            }

        }
        else
        {
            x = false;
        }
    }
}
