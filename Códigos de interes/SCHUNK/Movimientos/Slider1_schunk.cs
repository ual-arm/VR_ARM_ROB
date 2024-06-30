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
        // Este m�todo se llama cada vez que el valor del slider cambia
        Debug.Log("Valor del slider 1 cambiado: " + value);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Este m�todo se llama cuando el puntero del mouse entra en el �rea del slider
        Debug.Log("Mouse entra en el slider 1.");
        isInteracting = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Este m�todo se llama cuando el puntero del mouse sale del �rea del slider
        Debug.Log("Mouse sale del slider 1.");
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
            // Si el usuario no est� interactuando con el slider
            // Puedes hacer lo que necesites aqu�
            angulo1 = obtenerRotacionScript.ObtenerAnguloRotacion1();
            slider.value = angulo1;
        }
    }
}
