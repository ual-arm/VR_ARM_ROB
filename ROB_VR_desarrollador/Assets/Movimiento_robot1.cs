using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movimiento_robot_irb120 : MonoBehaviour
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

    private float minAngle1 =-163f;
    private float maxAngle1 =163f;

    private float minAngle2 =-108f;
    private float maxAngle2 =108f;

    private float minAngle3 = -68f;
    private float maxAngle3 = 108f;

    private float minAngle4 =-89f;
    private float maxAngle4 =89f;

    private float minAngle5 =-118f;
    private float maxAngle5 =118f;

    private float minAngle6 = -398f;
    private float maxAngle6 = 398f;

    private Vector3 homeRotation = Vector3.zero;

    float epsilon = 0.1f;

    // Velocidad de interpolación
    public float interpolationSpeed = 5f;

    // Variable para realizar la interpolación
    private bool interpolatingToHome = false;

    // Start is called before the first frame update
    void Start()
    {
        Mov1Angle = 0f;
        Mov2Angle = 0f;
        Mov3Angle = 0f;
        Mov4Angle = 0f;
        Mov5Angle = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Mov1Angle = Mathf.Clamp(Mov1Angle, minAngle1, maxAngle1);
        Mov2Angle = Mathf.Clamp(Mov2Angle, minAngle2, maxAngle2);
        Mov3Angle = Mathf.Clamp(Mov3Angle, minAngle3, maxAngle3);
        Mov4Angle = Mathf.Clamp(Mov4Angle, minAngle4, maxAngle4);
        Mov5Angle = Mathf.Clamp(Mov5Angle, minAngle5, maxAngle5);

        Mov1.transform.localRotation = Quaternion.AngleAxis(Mov1Angle, new Vector3(0, 1, 0));
        Mov2.transform.localRotation = Quaternion.AngleAxis(Mov2Angle, new Vector3(0, 0, 1));
        Mov3.transform.localRotation = Quaternion.AngleAxis(Mov3Angle, new Vector3(0, 0, 1));
        Mov4.transform.localRotation = Quaternion.AngleAxis(Mov4Angle, new Vector3(1, 0, 0));
        Mov5.transform.localRotation = Quaternion.AngleAxis(Mov5Angle, new Vector3(0, 0, 1));


        if (Input.GetKeyDown(KeyCode.H)|| interpolatingToHome == true)
        {
            interpolatingToHome = true;



            if (interpolatingToHome==true)
            {
                Debug.Log("Interpolando hacia home");

                // Interpolar hacia el punto de inicio (home)
                Mov1Angle = Mathf.Lerp(Mov1Angle, 0f, Time.deltaTime * interpolationSpeed);
                Mov2Angle = Mathf.Lerp(Mov2Angle, 0f, Time.deltaTime * interpolationSpeed);
                Mov3Angle = Mathf.Lerp(Mov3Angle, 0f, Time.deltaTime * interpolationSpeed);
                Mov4Angle = Mathf.Lerp(Mov4Angle, 0f, Time.deltaTime * interpolationSpeed);
                Mov5Angle = Mathf.Lerp(Mov5Angle, 0f, Time.deltaTime * interpolationSpeed);

                // Detener la interpolación cuando estamos lo suficientemente cerca del punto de inicio
                if (Mathf.Abs(Mov1Angle) < epsilon && Mathf.Abs(Mov2Angle) < epsilon &&
                           Mathf.Abs(Mov3Angle) < epsilon && Mathf.Abs(Mov4Angle) < epsilon &&
                           Mathf.Abs(Mov5Angle) < epsilon)
                {
                    interpolatingToHome = false;

                    Mov1Angle = 0f;
                    Mov2Angle = 0f;
                    Mov3Angle = 0f;
                    Mov4Angle = 0f;
                    Mov5Angle = 0f;
                }
            }


            // Mostrar mensaje en consola
            Debug.Log("El robot ha vuelto a la posición de Home.");
        }
    }
}
