using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slider1_schunk : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Slider slider;

    public Obtener_rotacion_schunk obtenerRotacionScript;
    // public Prueba2_slider Script_cambio;

    float valorSlider;





    public Transform articulacion1;
    private float angulo1;
    private Vector3 eje1;

    private bool isInteracting = false;

    private void Start()
    {
        // Agregar un listener para el evento de cambio de valor del slider
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // Este método se llama cada vez que el valor del slider cambia
        Debug.Log("Valor del slider 1 cambiado: " + value);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Este método se llama cuando el puntero del mouse entra en el área del slider
        Debug.Log("Mouse entra en el slider 1.");
        isInteracting = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Este método se llama cuando el puntero del mouse sale del área del slider
        Debug.Log("Mouse sale del slider 1.");
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

            angulo1 = slider.value;

            if (angulo1 < 0)
            {
                eje1 = new Vector3(0, 0, 1);
            }
            else
            {
                eje1 = new Vector3(0, 0, -1);
            }
            Quaternion rotacion1 = Quaternion.AngleAxis(Mathf.Abs(angulo1), eje1);
            articulacion1.localRotation = rotacion1;

            // }
        }
        else
        {
            // Si el usuario no está interactuando con el slider
            // Puedes hacer lo que necesites aquí
            angulo1 = obtenerRotacionScript.ObtenerAnguloRotacion1();
            slider.value = angulo1;
        }
    }
}
