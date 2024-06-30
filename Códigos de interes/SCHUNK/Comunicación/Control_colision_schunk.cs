using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
public class Control_colision_schunk : MonoBehaviour
{


    public GameObject pieza1;
    public GameObject pieza2;
    public GameObject pieza3;
    public GameObject pieza4;
    public GameObject pieza5;
    public GameObject pieza6;
    public GameObject pieza7;
    public GameObject mesa;
    public GameObject pinza;
    public GameObject pinza1;

    public float interpolationSpeed = 5f;

    private bool colision;
    private bool colision_robot;
    private bool colision_pinza;
    public bool colision_robot_mesa;
    public bool colision_pinza_mesa;

    private float J1;
    private float J2;
    private float J3;
    private float J4;
    private float J5;
    private float J6;
    private float J7;

    public GameObject agarre;
    public GameObject panel;



    void Start()
    {

        colision = false;
        colision_robot = false;
        colision_pinza = false;

    }

    // Update is called once per frame
    void Update()
    {

        //Compruevo si existen colisiones indeseadas entre cualquier punto del robot.


        colision = HayColision(pieza1, pieza4) ||
                    HayColision(pieza1, pieza5) ||
                    HayColision(pieza1, pieza6) ||
                    HayColision(pieza1, pieza7) ||
                    HayColision(pieza2, pieza4) ||
                    HayColision(pieza2, pieza7) ||
                    HayColision(pinza, pieza1) ||
                    HayColision(pinza, pieza2) ||
                    HayColision(pinza, pieza3) ||
                    HayColision(pinza, pieza5) ||
                    HayColision(pinza1, pieza1) ||
                    HayColision(pinza1, pieza2) ||
                    HayColision(pinza1, pieza3) ||
                    HayColision(pinza1, pieza5) ||
                    HayColision(mesa, pieza4) ||
                    HayColision(mesa, pieza5) ||
                    HayColision(mesa, pieza6) ||
                    HayColision(mesa, pieza7) ||
                    HayColision(mesa, pinza) ||
                    HayColision(mesa, pinza1);




        if (colision == true)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
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
