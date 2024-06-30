using UnityEngine;

public class MoverObjetoVR : MonoBehaviour
{
    public float velocidadRotacion = 50f;
    public Transform objetoARotar;

    void Update()
    {
        // Detectar la entrada del joystick en el eje horizontal
        float inputHorizontal = Input.GetAxis("Horizontal");

        // Calcular la cantidad de rotación basada en la entrada del joystick
        float rotacion = inputHorizontal * velocidadRotacion * Time.deltaTime;
        Debug.Log("Valor de entrada horizontal: " + inputHorizontal);

        // Rotar el objeto sobre su eje x
        objetoARotar.Rotate(rotacion, 0f, 0f);
    }
}