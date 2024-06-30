using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slider3_irb140 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Slider slider;

    public Obtener_rotacion obtenerRotacionScript;
    // public Prueba2_slider Script_cambio;

    float valorSlider;





    public Transform articulacion3;
    private float angulo3;
    private Vector3 eje3;

    private bool isInteracting = false;

    private void Start()
    {
        // Agregar un listener para el evento de cambio de valor del slider
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // Este método se llama cada vez que el valor del slider cambia
        Debug.Log("Valor del slider cambiado: " + value);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Este método se llama cuando el puntero del mouse entra en el área del slider
        Debug.Log("Mouse entra en el slider.");
        isInteracting = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Este método se llama cuando el puntero del mouse sale del área del slider
        Debug.Log("Mouse sale del slider.");
        isInteracting = false;
    }

    private void Update()
    {
        // Ejemplo de cómo puedes usar la variable isInteracting en una condición
        if (isInteracting)
        {
            // Si el usuario está interactuando con el slider
            // Puedes hacer lo que necesites aquí
            // if (Script_cambio.enabled)
            //{

            angulo3 = slider.value;

            if (angulo3 < 0)
            {
                eje3 = new Vector3(0, 1, 0);
            }
            else
            {
                eje3 = new Vector3(0, -1, 0);
            }
            Quaternion rotacion3 = Quaternion.AngleAxis(Mathf.Abs(angulo3), eje3);
            articulacion3.localRotation = rotacion3;

            // }
        }
        else
        {
            // Si el usuario no está interactuando con el slider
            // Puedes hacer lo que necesites aquí
            angulo3 = obtenerRotacionScript.ObtenerAnguloRotacion3();
            slider.value = angulo3;
        }
    }
}

