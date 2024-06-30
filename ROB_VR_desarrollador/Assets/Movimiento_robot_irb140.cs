using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
public class Movimiento_robot_irb140 : MonoBehaviour
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

    public GameObject Mov6;
    public float Mov6Angle;

    private float minAngle1 = -178f;
    private float maxAngle1 = 178f;

    private float minAngle2 = -108f;
    private float maxAngle2 = 88f;

    private float minAngle3 = -48f;
    private float maxAngle3 = 228f;

    private float minAngle4 = -198f;
    private float maxAngle4 = 198f;

    private float minAngle5 = -113f;
    private float maxAngle5 = 113f;

    private float minAngle6 = -398f;
    private float maxAngle6 = 398f;

    private float Mov1Angleanterior;
    private float Mov2Angleanterior;
    private float Mov3Angleanterior;
    private float Mov4Angleanterior;
    private float Mov5Angleanterior;
    private float Mov6Angleanterior;

    private Vector3 homeRotation = Vector3.zero;

    float epsilon = 0.1f;

    // Velocidad de interpolación
    public float interpolationSpeed = 5f;

    // Variable para realizar la interpolación
    private bool interpolatingToHome = false;

    public static float mov1Angles;
    public static float mov2Angles;
    public static float mov3Angles;
    public static float mov4Angles;
    public static float mov5Angles;
    public static float mov6Angles;

    public GameObject pieza1;
    public GameObject pieza2;
    public GameObject pieza3;
    public GameObject pieza4;
    public GameObject pieza5;
    public GameObject pieza6;
    public GameObject pieza7;
    public GameObject mesa;


    public bool colision;

    // Start is called before the first frame update
    void Start()
    {
        Mov1Angle = 100f;
        Mov2Angle = 0f;
        Mov3Angle = 0f;
        Mov4Angle = 0f;
        Mov5Angle = 0f;
        Mov6Angle = 0f;
        colision = false;

    }

    // Update is called once per frame

    void Update()
    {
             Mov1Angle = Mathf.Clamp(Mov1Angle, minAngle1, maxAngle1);
             Mov2Angle = Mathf.Clamp(Mov2Angle, minAngle2, maxAngle2);
             Mov3Angle = Mathf.Clamp(Mov3Angle, minAngle3, maxAngle3);
             Mov4Angle = Mathf.Clamp(Mov4Angle, minAngle4, maxAngle4);
             Mov5Angle = Mathf.Clamp(Mov5Angle, minAngle5, maxAngle5);
             Mov6Angle = Mathf.Clamp(Mov6Angle, minAngle6, maxAngle6);

             Mov1.transform.localRotation = Quaternion.AngleAxis(Mov1Angle, new Vector3(0, 0, 1));
             Mov2.transform.localRotation = Quaternion.AngleAxis(Mov2Angle, new Vector3(0, 1, 0));
             Mov3.transform.localRotation = Quaternion.AngleAxis(Mov3Angle, new Vector3(0, 1, 0));
             Mov4.transform.localRotation = Quaternion.AngleAxis(Mov4Angle, new Vector3(1, 0, 0));
             Mov5.transform.localRotation = Quaternion.AngleAxis(Mov5Angle, new Vector3(0, 1, 0));
             Mov6.transform.localRotation = Quaternion.AngleAxis(Mov6Angle, new Vector3(1, 0, 0));

             colision =  HayColision(pieza7, mesa)   ||
                         HayColision(pieza7, pieza1) ||
                         HayColision(pieza7, pieza2) ||
                         HayColision(pieza5, pieza2) ||
                         HayColision(pieza5, mesa)   ||
                         HayColision(pieza5, pieza1) ||
                         HayColision(pieza6, pieza2) ||
                         HayColision(pieza6, pieza1);

             if(colision==true){
                Debug.Log("Existe colision con el robot");

                 if (Mov2Angle <= 0)
                 {
                     Mov2Angle = Mov2Angle + 10;
                 }
                 else
                 {
                     Mov2Angle = Mov2Angle - 10;
                 }

                Mov2.transform.localRotation = Quaternion.AngleAxis(Mov2Angle, new Vector3(0, 1, 0));


             }


             mov1Angles = Mov1.transform.localRotation.z;
             mov2Angles = Mov2.transform.localRotation.y;
             mov3Angles = Mov3.transform.localRotation.y;
             mov4Angles = Mov4Angle;
             mov5Angles = Mov5.transform.localRotation.y;
             mov6Angles = Mov6Angle;

             if (Input.GetKeyDown(KeyCode.H) || interpolatingToHome == true)
             {
                 interpolatingToHome = true;



                 if (interpolatingToHome == true)
                 {
                     Debug.Log("Interpolando hacia home");

                     // Interpolar hacia el punto de inicio (home)
                    Mov1Angle = Mathf.Lerp(Mov1Angle, 0f, Time.deltaTime * interpolationSpeed);
                    Mov2Angle = Mathf.Lerp(Mov2Angle, 0f, Time.deltaTime * interpolationSpeed);
                    Mov3Angle = Mathf.Lerp(Mov3Angle, 0f, Time.deltaTime * interpolationSpeed);
                    Mov4Angle = Mathf.Lerp(Mov4Angle, 0f, Time.deltaTime * interpolationSpeed);
                    Mov5Angle = Mathf.Lerp(Mov5Angle, 0f, Time.deltaTime * interpolationSpeed);
                    Mov6Angle = Mathf.Lerp(Mov6Angle, 0f, Time.deltaTime * interpolationSpeed);

                    // Detener la interpolación cuando estamos lo suficientemente cerca del punto de inicio
                    if (Mathf.Abs(Mov1Angle) < epsilon && Mathf.Abs(Mov2Angle) < epsilon &&
                               Mathf.Abs(Mov3Angle) < epsilon && Mathf.Abs(Mov4Angle) < epsilon &&
                               Mathf.Abs(Mov5Angle) < epsilon && Mathf.Abs(Mov6Angle) < epsilon)
                    {
                        interpolatingToHome = false;

                        Mov1Angle = 0f;
                        Mov2Angle = 0f;
                        Mov3Angle = 0f;
                        Mov4Angle = 0f;
                        Mov5Angle = 0f;
                        Mov6Angle = 0f;
                    }
                 }


                 // Mostrar mensaje en consola
                 Debug.Log("El robot ha vuelto a la posición de Home.");
             }
    }

    private bool HayColision(GameObject objetoA, GameObject objetoB)
    {
        Collider colliderA = objetoA.GetComponent<Collider>();
        Collider colliderB = objetoB.GetComponent<Collider>();

        if (colliderA != null && colliderB != null)
        {
            return colliderA.bounds.Intersects(colliderB.bounds);
        }

        return false;

    }



}
