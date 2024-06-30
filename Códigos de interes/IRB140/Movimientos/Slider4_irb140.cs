using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slider4_irb140 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Slider slider;

    public Obtener_rotacion obtenerRotacionScript;
    // public Prueba2_slider Script_cambio;

    float valorSlider;





    public Transform articulacion4;
    private float angulo4;
    private Vector3 eje4;

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

            angulo4 = slider.value;

            if (angulo4 < 0)
            {
                eje4 = new Vector3(-1, 0, 0);
            }
            else
            {
                eje4 = new Vector3(1, 0, 0);
            }
            Quaternion rotacion4 = Quaternion.AngleAxis(Mathf.Abs(angulo4), eje4);
            articulacion4.localRotation = rotacion4;

            // }
        }
        else
        {
            // Si el usuario no está interactuando con el slider
            // Puedes hacer lo que necesites aquí
            angulo4 = obtenerRotacionScript.ObtenerAnguloRotacion4();
            slider.value = angulo4;
        }
    }
}

