using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slider5_irb190 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Slider slider;

    public Obtener_rotacion_irb1090 obtenerRotacionScript;
    // public Prueba2_slider Script_cambio;

    float valorSlider;





    public Transform articulacion5;
    private float angulo5;
    private Vector3 eje5;

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

            angulo5 = slider.value;

            if (angulo5 < 0)
            {
                eje5 = new Vector3(0, 1, 0);
            }
            else
            {
                eje5 = new Vector3(0, -1, 0);
            }
            Quaternion rotacion5 = Quaternion.AngleAxis(Mathf.Abs(angulo5), eje5);
            articulacion5.localRotation = rotacion5;

            // }
        }
        else
        {
            // Si el usuario no está interactuando con el slider
            // Puedes hacer lo que necesites aquí
            angulo5 = obtenerRotacionScript.ObtenerAnguloRotacion5();
            slider.value = angulo5;
        }
    }
}