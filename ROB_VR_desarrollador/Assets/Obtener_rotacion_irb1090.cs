using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obtener_rotacion_irb1090 : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform articulacion1;
    public Transform articulacion2;
    public Transform articulacion3;
    public Transform articulacion4;
    public Transform articulacion5;
    public Transform articulacion6;

    private float angulo1;
    private float angulo2;
    private float angulo3;
    private float angulo4;
    private float angulo5;
    private float angulo6;

    private Vector3 eje1;
    private Vector3 eje2;
    private Vector3 eje3;
    private Vector3 eje4;
    private Vector3 eje5;
    private Vector3 eje6;

    // Update is called once per frame
    void Update()
    {
        // Obtener la rotaci�n local del objeto
        Quaternion rotation1 = articulacion1.localRotation;
        rotation1.ToAngleAxis(out angulo1, out eje1);
        Quaternion rotation2 = articulacion2.localRotation;
        rotation2.ToAngleAxis(out angulo2, out eje2);
        Quaternion rotation3 = articulacion3.localRotation;
        rotation3.ToAngleAxis(out angulo3, out eje3);
        Quaternion rotation4 = articulacion4.localRotation;
        rotation4.ToAngleAxis(out angulo4, out eje4);
        Quaternion rotation5 = articulacion5.localRotation;
        rotation5.ToAngleAxis(out angulo5, out eje5);
        Quaternion rotation6 = articulacion6.localRotation;
        rotation6.ToAngleAxis(out angulo6, out eje6);


        if (eje1.z > 0)
        {
            angulo1 = -angulo1;
        }
        if (eje2.y > 0)
        {
            angulo2 = -angulo2;
        }
        if (eje3.y > 0)
        {
            angulo3 = -angulo3;
        }
        if (eje4.x < 0)
        {
            angulo4 = -angulo4;
        }
        if (eje5.y > 0)
        {
            angulo5 = -angulo5;
        }
        if (eje6.x < 0)
        {
            angulo6 = -angulo6;
        }

        //Debug.Log("�ngulo de rotaci�n1: " + angulo1 + " grados");
        //Debug.Log("Eje de rotaci�n: " + eje1);
        //Debug.Log("�ngulo de rotaci�n2: " + angulo2 + " grados");
        //Debug.Log("Eje de rotaci�n: " + eje2);
        //Debug.Log("�ngulo de rotaci�n3: " + angulo3 + " grados");
        //Debug.Log("Eje de rotaci�n: " + eje3);
        //Debug.Log("�ngulo de rotaci�n4: " + angulo4 + " grados");
        //Debug.Log("Eje de rotaci�n: " + eje4);
        //Debug.Log("�ngulo de rotaci�n5: " + angulo5 + " grados");
        //Debug.Log("Eje de rotaci�n: " + eje5);
        //Debug.Log("�ngulo de rotaci�n6: " + angulo6 + " grados");
        //Debug.Log("Eje de rotaci�n: " + eje6);


        // Puedes mostrar la informaci�n en cualquier otro lugar, como en un TextMeshProUGUI
        // textMeshProUGUIComponent.text = "�ngulo de rotaci�n: " + angle + " grados\nEje de rotaci�n: " + axis;
    }

    public float ObtenerAnguloRotacion1()
    {
        return angulo1;
    }
    public float ObtenerAnguloRotacion2()
    {
        return angulo2;
    }
    public float ObtenerAnguloRotacion3()
    {
        return angulo3;
    }
    public float ObtenerAnguloRotacion4()
    {
        return angulo4;
    }
    public float ObtenerAnguloRotacion5()
    {
        return angulo5;
    }
    public float ObtenerAnguloRotacion6()
    {
        return angulo6;
    }

}

