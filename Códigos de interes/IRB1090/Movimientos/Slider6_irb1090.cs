using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slider6_irb1090 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Slider slider;

    public Obtener_rotacion_irb1090 obtenerRotacionScript;
    // public Prueba2_slider Script_cambio;

    float valorSlider;





    public Transform articulacion6;
    private float angulo6;
    private Vector3 eje6;

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

            angulo6 = slider.value;

            if (angulo6 < 0)
            {
                eje6 = new Vector3(-1, 0, 0);
            }
            else
            {
                eje6 = new Vector3(1, 0, 0);
            }
            Quaternion rotacion6 = Quaternion.AngleAxis(Mathf.Abs(angulo6), eje6);
            articulacion6.localRotation = rotacion6;

            // }
        }
        else
        {
            // Si el usuario no está interactuando con el slider
            // Puedes hacer lo que necesites aquí
            angulo6 = obtenerRotacionScript.ObtenerAnguloRotacion6();
            slider.value = angulo6;
        }
    }
}


