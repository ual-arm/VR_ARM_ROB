using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
public class DropdownOptionManager_irb1090 : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    public Canvas manual;
    public Canvas puntos;
    public Canvas trayectorias;
    public Canvas programas;

    public TMP_DropdownManager_irb1090 dropdownManagerpuntos;
    public Control_trayectoria_irb1090 dropdownManagertrayectorias;
    public Comunicacion_irb1090 comunicacion_Irb140;


    void Start()
    {
        // Define las opciones del dropdown
        DefineOpcionesDropdown();
    }

    void DefineOpcionesDropdown()
    {
        // Limpia todas las opciones existentes en el dropdown
        dropdown.ClearOptions();

        // Crea una lista para almacenar las opciones del dropdown
        var options = new List<TMP_Dropdown.OptionData>();

        // Agrega las opciones deseadas a la lista
        options.Add(new TMP_Dropdown.OptionData("Movimiento Directo"));
        options.Add(new TMP_Dropdown.OptionData("Puntos"));
        options.Add(new TMP_Dropdown.OptionData("Trayectorias"));
        options.Add(new TMP_Dropdown.OptionData("Programas"));

        // Asigna las opciones al dropdown
        dropdown.AddOptions(options);

        // Registra un método para ser llamado cuando cambia la selección del dropdown
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        int opcionSeleccionada = change.value;

        // Llama a la función correspondiente según la opción seleccionada
        switch (opcionSeleccionada)
        {
            case 0:
                Manual();
                break;
            case 1:
                Puntos();
                break;
            case 2:
                Trayectorias();
                break;
            case 3:
                Programas();
                break;
            default:
                Debug.LogError("Opción no válida seleccionada.");
                break;
        }
    }

    void Manual()
    {
        Debug.Log("Función 1 seleccionada.");
        // Agrega aquí la lógica para la Función 1

        manual.enabled = true;
        puntos.enabled = false;
        trayectorias.enabled = false;
        programas.enabled = false;

        //comunicacion_Irb140.Ejecutar_manual();

    }

    void Puntos()
    {
        Debug.Log("Función 2 seleccionada.");
        // Agrega aquí la lógica para la Función 2

        manual.enabled = false;
        puntos.enabled = true;
        trayectorias.enabled = false;
        programas.enabled = false;

        dropdownManagerpuntos.CopyOptions();

    }

    void Trayectorias()
    {
        Debug.Log("Función 3 seleccionada.");
        // Agrega aquí la lógica para la Función 3

        manual.enabled = false;
        puntos.enabled = false;
        trayectorias.enabled = true;
        programas.enabled = false;

        dropdownManagertrayectorias.CopyOptions();

    }

    void Programas()
    {
        Debug.Log("Función 4 seleccionada.");
        // Agrega aquí la lógica para la Función 4

        manual.enabled = false;
        puntos.enabled = false;
        trayectorias.enabled = false;
        programas.enabled = true;

    }

    public void SetDropdownToManual()
    {
        dropdown.value = 0; // Indice de la opción "Manual"
        Manual(); // Llama a la función Manual para realizar las acciones necesarias
    }
}