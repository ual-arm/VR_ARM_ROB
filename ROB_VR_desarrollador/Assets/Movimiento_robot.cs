using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento_robot : MonoBehaviour
{
    public GameObject Mov1;
    public float Mov1Angle;

    public GameObject Mov2;
    public float Mov2Angle;

    public GameObject Mov3;
    public float Mov3Angle;

    public GameObject Mov4;
    public float Mov4Angle;

    public GameObject Mov5;
    public float Mov5Angle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Mov1.transform.localRotation = Quaternion.AngleAxis(Mov1Angle, new Vector3(0, 1, 0));
        Mov2.transform.localRotation = Quaternion.AngleAxis(Mov2Angle, new Vector3(0, 0, 1));
        Mov3.transform.localRotation = Quaternion.AngleAxis(Mov3Angle, new Vector3(0, 0, 1));
        Mov4.transform.localRotation = Quaternion.AngleAxis(Mov4Angle, new Vector3(1, 0, 0));
        Mov5.transform.localRotation = Quaternion.AngleAxis(Mov5Angle, new Vector3(0, 0, 1));

    }
}
