using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slider2_irb140 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Slider slider;

    public Obtener_rotacion obtenerRotacionScript;
    // public Prueba2_slider Script_cambio;

    float valorSlider;





    public Transform articulacion2;
    private float angulo2;
    private Vector3 eje2;

    private bool isInteracting = false;

    private void Start()
    {
        // Agregar un listener para el evento de cambio de valor del slider
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // Este m�todo se llama cada vez que el valor del slider cambia
        Debug.Log("Valor del slider 2 cambiado: " + value);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Este m�todo se llama cuando el puntero del mouse entra en el �rea del slider
        Debug.Log("Mouse entra en el slider 2.");
        isInteracting = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Este m�todo se llama cuando el puntero del mouse sale del �rea del slider
        Debug.Log("Mouse sale del slider 2.");
        isInteracting = false;
    }

    private void Update()
    {
        // Ejemplo de c�mo puedes usar la variable isInteracting en una condici�n
        if (isInteracting)
        {
            // Si el usuario est� interactuando con el slider
            // Puedes hacer lo que necesites aqu�
            // if (Script_cambio.enabled)
            //{

            angulo2 = slider.value;

            if (angulo2 < 0)
            {
                eje2 = new Vector3(0, 1, 0);
            }
            else
            {
                eje2 = new Vector3(0, -1, 0);
            }
            Quaternion rotacion2 = Quaternion.AngleAxis(Mathf.Abs(angulo2), eje2);
            articulacion2.localRotation = rotacion2;

            // }
        }
        else
        {
            // Si el usuario no est� interactuando con el slider
            // Puedes hacer lo que necesites aqu�
            angulo2 = obtenerRotacionScript.ObtenerAnguloRotacion2();
            slider.value = angulo2;
        }
    }
}

