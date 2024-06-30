using UnityEngine;
using TMPro;

public class InputFieldManager : MonoBehaviour
{
    public TMP_InputField inputField1;
    public TMP_InputField inputField2;

    void Start()
    {
        // Suscribirse al evento onValueChanged del primer InputField
        inputField1.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    void OnInputFieldValueChanged(string newValue)
    {
        // Cuando cambia el valor del primer InputField, actualizar el valor del segundo InputField
        inputField2.text = newValue;
    }
}
