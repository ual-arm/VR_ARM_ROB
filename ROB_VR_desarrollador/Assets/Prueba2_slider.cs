using UnityEngine;
using UnityEngine.UI;
using UltimateXR.Core.Components;
using UltimateXR.Core;

public class Prueba2_slider : MonoBehaviour
{
    // Referencia al slider de Ultimate XR
    public Obtener_rotacion obtenerRotacionScript;

    public Prueba2_slider Script_cambio;

    public Slider slider;
    float valorSlider;





    public Transform articulacion1;
    private float angulo1;
    private Vector3 eje1;


    // Método para usar el valor del slider
    void Update()
    {












        // Obtener el valor actual del slider

        //angulo1 = obtenerRotacionScript.ObtenerAnguloRotacion1();
        //slider.value = angulo1;
        //if (Script_cambio.enabled) { 

        //     angulo1 = slider.value;

         //   if (angulo1 < 0)
         //   {
         //       eje1 = new Vector3(0, 0, 1);
         //   }
         //   else
         //   {
         //       eje1 = new Vector3(0, 0, -1);
         //   }
         //   Quaternion rotacion1 = Quaternion.AngleAxis(Mathf.Abs(angulo1), eje1);
         //   articulacion1.localRotation = rotacion1;

       // }


        // Hacer algo con el valor del slider
        //Debug.Log("El valor del slider es: " + valorSlider);
    }
}
