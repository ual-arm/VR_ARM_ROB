using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalizarConexion_irb : MonoBehaviour
{
    
    public Comunicacion_irb140 Comunicacion_irb140;
    // Start is called before the first frame update

    private Inicio_Comunicacion_irb140 Inicio;

    void Start()
    {
        //Comunicacion = GetComponent<Comunicacion>();
    }

    // Update is called once per frame
    void Update()
    {

        //FinalizarConexion();

        if (Inicio != null)
        {
            Comunicacion_irb140.FinalizarConexion();

        }

    }

    void FinalizarConexion()
    {
        if (Inicio!=null) { 
            Comunicacion_irb140.FinalizarConexion(); 
        
        }
        
    }
}