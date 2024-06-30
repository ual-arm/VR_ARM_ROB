using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{

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

    public Transform[] articulaciones = new Transform[6];

    // Start is called before the first frame update
    public void MoveToHome()
    {
    

            Quaternion[] rotacionesDeseadas = new Quaternion[6];
            Vector3[] ejes = new Vector3[6];

            // Obtener los valores de rotación utilizando el script obtenerRotacionScript
            angulo1 = 0;
            if (angulo1 < 0)
            {
                eje1 = new Vector3(0, 0, 1);
            }
            else
            {
                eje1 = new Vector3(0, 0, -1);
            }
            rotacionesDeseadas[0] = Quaternion.AngleAxis(Mathf.Abs(angulo1), eje1);

            angulo2 = 0;
        if (angulo2 < 0)
            {
                eje2 = new Vector3(0, 1, 0);
            }
            else
            {
                eje2 = new Vector3(0, -1, 0);
            }
            rotacionesDeseadas[1] = Quaternion.AngleAxis(Mathf.Abs(angulo2), eje2);

            angulo3 = 0;

        if (angulo3 < 0)
            {
                eje3 = new Vector3(0, 1, 0);
            }
            else
            {
                eje3 = new Vector3(0, -1, 0);
            }
            rotacionesDeseadas[2] = Quaternion.AngleAxis(Mathf.Abs(angulo3), eje3);

            angulo4 = 0;
        if (angulo4 < 0)
            {
                eje4 = new Vector3(-1, 0, 0);
            }
            else
            {
                eje4 = new Vector3(1, 0, 0);
            }
            rotacionesDeseadas[3] = Quaternion.AngleAxis(Mathf.Abs(angulo4), eje4);

            angulo5 = 0;
        if (angulo5 < 0)
            {
                eje5 = new Vector3(0, 1, 0);
            }
            else
            {
                eje5 = new Vector3(0, -1, 0);
            }
            rotacionesDeseadas[4] = Quaternion.AngleAxis(Mathf.Abs(angulo5), eje5);

            angulo6 = 0;
        if (angulo6 < 0)
            {
                eje6 = new Vector3(-1, 0, 0);
            }
            else
            {
                eje6 = new Vector3(1, 0, 0);
            }

            rotacionesDeseadas[5] = Quaternion.AngleAxis(Mathf.Abs(angulo6), eje6);


            // Inicia la rotación gradual para todas las articulaciones
            StartCoroutine(RotacionGradual(rotacionesDeseadas));
      
    }


    IEnumerator RotacionGradual(Quaternion[] rotacionesDeseadas)
    {
        float duracionRotacion = 3.0f; // Duración de la rotación gradual
        float tiempoPasado = 0.0f; // Tiempo transcurrido desde el inicio de la rotación gradual


        Quaternion[] rotacionesIniciales = new Quaternion[6];
        for (int i = 0; i < 6; i++)
        {
            rotacionesIniciales[i] = articulaciones[i].localRotation;
        }

        while (tiempoPasado < duracionRotacion)
        {
            tiempoPasado += Time.deltaTime;
            float t = Mathf.Clamp01(tiempoPasado / duracionRotacion);

            // Interpola gradualmente entre las rotaciones iniciales y las rotaciones deseadas para todas las articulaciones
            for (int i = 0; i < 6; i++)
            {
                articulaciones[i].localRotation = Quaternion.Lerp(rotacionesIniciales[i], rotacionesDeseadas[i], t);
            }

            yield return null; // Espera un frame
        }

        // Asigna las rotaciones deseadas al finalizar la interpolación para asegurar que sean exactas
        for (int i = 0; i < 6; i++)
        {
            articulaciones[i].localRotation = rotacionesDeseadas[i];
        }


    }

}
