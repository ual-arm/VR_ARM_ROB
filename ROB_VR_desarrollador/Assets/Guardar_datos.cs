using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardar_datos : MonoBehaviour
{
    // Start is called before the first frame update
    private TMP_DropdownManager dropdownManager;

    public void guardar()
    {
        // Obtener una referencia al GameObject que tiene adjunto el script TMP_DropdownManager
        GameObject dropdownManagerObject = GameObject.Find("Puntos");

        // Obtener una referencia al script TMP_DropdownManager
        dropdownManager = dropdownManagerObject.GetComponent<TMP_DropdownManager>();

        // Verificar si la referencia es nula
        if (dropdownManager == null)
        {
            Debug.LogError("No se pudo encontrar el script TMP_DropdownManager.");
            return;
        }

        // Ahora puedes acceder a los puntos desde el script TMP_DropdownManager
        foreach (TMP_DropdownManager.DropdownOption option in dropdownManager.options)
        {
            Debug.Log("Nombre de la opción: " + option.name);
            Debug.Log("Valor 1: " + option.value1);
            //Debug.Log("Valor 2: " + option.value2);
            // Continuar con los demás valores si es necesario
        }
    }
}
