using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover_pantalla_irb140 : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public Vector3 position1A;
    public Vector3 position1B;
    public Vector3 position2A;
    public Vector3 position2B;
    public Vector3 position3A;
    public Vector3 position3B;
    public Vector3 position0;


    public void Positioninicial()
    {
        if (object1 != null)
        {
            object1.transform.localPosition = position0;
            object1.transform.localRotation = Quaternion.Euler(0, 0, 60);
        }
        if (object2 != null)
        {
            //object2.

        }
    }

    public void Positionirb_140()
    {
        if (object1 != null)
        {
            object1.transform.localPosition = position1A;
            object1.transform.localRotation = Quaternion.Euler(0, -20, 90);
        }

        if (object2 != null)
        {
            object2.transform.localPosition = position1B;
            object2.transform.localRotation = Quaternion.Euler(0, 20, 90);
        }
    }

    public void Positionirb_1090()
    {
        if (object1 != null)
        {
            object1.transform.localPosition = position2A;
            object1.transform.localRotation = Quaternion.Euler(0, -20, 90);
        }

        if (object2 != null)
        {
            object2.transform.localPosition = position2B;
            object2.transform.localRotation = Quaternion.Euler(0, 20, 90);
        }
    }

    public void Positionirb_schunk()
    {
        if (object1 != null)
        {
            object1.transform.localPosition = position3A;
            object1.transform.localRotation = Quaternion.Euler(0, -20, 90);
        }

        if (object2 != null)
        {
            object2.transform.localPosition = position3B;
            object2.transform.localRotation = Quaternion.Euler(0, 20, 90);
        }
    }

}
