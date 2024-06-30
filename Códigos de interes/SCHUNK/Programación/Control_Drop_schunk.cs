using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class TMP_DropdownManager_schunk : MonoBehaviour
{
    // Referencia al TMP_Dropdown en tu escena
    public TMP_Dropdown dropdown;
    public TMP_Dropdown dropdown2;
    public Obtener_rotacion_schunk obtenerRotacionScript;

    public TMP_InputField campoEntrada;

    public float velocidad = 3;

    private bool pointReached = false;

    // Estructura para almacenar una opción del TMP_Dropdown junto con sus coordenadas
    public struct DropdownOption
    {
        public string name;
        public float value1;
        public float value2;
        public float value3;
        public float value4;
        public float value5;
        public float value6;

        public DropdownOption(string _name, float _value1, float _value2, float _value3, float _value4, float _value5, float _value6)
        {
            name = _name;
            value1 = _value1;
            value2 = _value2;
            value3 = _value3;
            value4 = _value4;
            value5 = _value5;
            value6 = _value6;
        }
    }

    // Opciones iniciales del TMP_Dropdown
    public List<DropdownOption> options = new List<DropdownOption>() {
        new DropdownOption("Home", 0f, 0f, 0f, 0f, 0f, 0f)
    };


    private float angulo1;
    private float angulo2;
    private float angulo3;
    private float angulo4;
    private float angulo5;
    private float angulo6;

    private Vector3 eje1;
    private Vector3 eje2;
    private Vector3 eje3;
    private Vector3 eje4;
    private Vector3 eje5;
    private Vector3 eje6;

    //public Transform articulacion1;
    //public Transform articulacion2;
    //public Transform articulacion3;
    //public Transform articulacion4;
    //public Transform articulacion5;
    //public Transform articulacion6;

    public Transform[] articulaciones = new Transform[6]; // Array que contiene todas las articulaciones

    public GameObject botonGrabarCanvas;
    public GameObject botonBorrarCanvas;
    public GameObject Comunicacion;
    public Toggle Comunicacion_;

    public Toggle Limite_velocidad;
    public Comunicacion_schunk comunicacionesScript;

    public void Start()
    {
        comunicacionesScript = Comunicacion.GetComponent<Comunicacion_schunk>();
    }

    // Método para agregar una nueva opción al TMP_Dropdown
    public void AddOption()
    {
        // Genera un nombre para la nueva opción
        string newOption = "Punto " + (dropdown.options.Count);

        // Crea un nuevo DropdownOption con valores predeterminados
        DropdownOption option = new DropdownOption(newOption, 0f, 0f, 0f, 0f, 0f, 0f);

        // Añade la nueva opción a la lista
        if (dropdown.options.Count > 0)
        {
            options.Add(option);
        }


        // Actualiza las opciones del TMP_Dropdown
        RefreshDropdownOptions();
    }

    // Método para actualizar las opciones del TMP_Dropdown
    public void RefreshDropdownOptions()
    {
        // Borra todas las opciones existentes en el TMP_Dropdown
        dropdown.ClearOptions();

        // Agrega las opciones actualizadas al TMP_Dropdown
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (DropdownOption option in options)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(option.name));
        }
        dropdown.AddOptions(dropdownOptions);

        Debug.Log("Nombre de la opción seleccionada: " + options[dropdown.value].name);

    }


    public void DropdownValueChanged(TMP_Dropdown change)
    {
        if (options[change.value].name == "Home")
        {
            botonGrabarCanvas.SetActive(false);
            botonBorrarCanvas.SetActive(false);
        }
        else
        {
            botonGrabarCanvas.SetActive(true);
            botonBorrarCanvas.SetActive(true);
        }
    }


    // Método para guardar los valores asociados a la opción seleccionada en el TMP_Dropdown
    public void SaveValues()
    {
        int selectedIndex = dropdown.value;
        if (selectedIndex >= 0 && selectedIndex < options.Count)
        {
            // Obtener los valores de rotación utilizando el script obtenerRotacionScript
            float value1 = obtenerRotacionScript.ObtenerAnguloRotacion1();
            float value2 = obtenerRotacionScript.ObtenerAnguloRotacion2();
            float value3 = obtenerRotacionScript.ObtenerAnguloRotacion3();
            float value4 = obtenerRotacionScript.ObtenerAnguloRotacion4();
            float value5 = obtenerRotacionScript.ObtenerAnguloRotacion5();
            float value6 = obtenerRotacionScript.ObtenerAnguloRotacion6();

            // Asigna los valores obtenidos a la opción seleccionada
            options[selectedIndex] = new DropdownOption(options[selectedIndex].name, value1, value2, value3, value4, value5, value6);

        }
        else
        {
            // Imprime un mensaje de error si no se ha seleccionado ninguna opción o si el índice está fuera de rango
            Debug.LogError("No se ha seleccionado ninguna opción o el índice está fuera de rango.");
        }
    }

    public void DeleteOption()
    {
        int selectedIndex = dropdown.value;
        if (selectedIndex > 0 && selectedIndex < options.Count)
        {
            if (options[selectedIndex].name != "Home")
            {
                // Eliminar la opción seleccionada
                options.RemoveAt(selectedIndex);

                // Renombrar las opciones restantes
                for (int i = selectedIndex; i < options.Count; i++)
                {
                    options[i] = new DropdownOption("Punto " + i, options[i].value1, options[i].value2, options[i].value3, options[i].value4, options[i].value5, options[i].value6);
                }

                // Actualizar las opciones del TMP_Dropdown
                RefreshDropdownOptions();
            }
            else
            {
                Debug.LogWarning("No se puede borrar la opción 'Home'.");
            }
        }
        else
        {
            Debug.LogWarning("No se ha seleccionado ninguna opción o el índice está fuera de rango.");
        }
    }

    public void MoveToPoint()
    {
        int selectedIndex = dropdown.value;
        if (selectedIndex >= 0 && selectedIndex < options.Count)
        {

            Quaternion[] rotacionesDeseadas = new Quaternion[6];
            Vector3[] ejes = new Vector3[6];

            // Obtener los valores de rotación utilizando el script obtenerRotacionScript
            angulo1 = options[selectedIndex].value1;
            if (angulo1 < 0)
            {
                eje1 = new Vector3(0, 0, 1);
            }
            else
            {
                eje1 = new Vector3(0, 0, -1);
            }
            rotacionesDeseadas[0] = Quaternion.AngleAxis(Mathf.Abs(angulo1), eje1);

            angulo2 = options[selectedIndex].value2;
            if (angulo2 < 0)
            {
                eje2 = new Vector3(0, 1, 0);
            }
            else
            {
                eje2 = new Vector3(0, -1, 0);
            }
            rotacionesDeseadas[1] = Quaternion.AngleAxis(Mathf.Abs(angulo2), eje2);

            angulo3 = options[selectedIndex].value3;

            if (angulo3 < 0)
            {
                eje3 = new Vector3(0, 1, 0);
            }
            else
            {
                eje3 = new Vector3(0, -1, 0);
            }
            rotacionesDeseadas[2] = Quaternion.AngleAxis(Mathf.Abs(angulo3), eje3);

            angulo4 = options[selectedIndex].value4;
            if (angulo4 < 0)
            {
                eje4 = new Vector3(-1, 0, 0);
            }
            else
            {
                eje4 = new Vector3(1, 0, 0);
            }
            rotacionesDeseadas[3] = Quaternion.AngleAxis(Mathf.Abs(angulo4), eje4);

            angulo5 = options[selectedIndex].value5;
            if (angulo5 < 0)
            {
                eje5 = new Vector3(0, 1, 0);
            }
            else
            {
                eje5 = new Vector3(0, -1, 0);
            }
            rotacionesDeseadas[4] = Quaternion.AngleAxis(Mathf.Abs(angulo5), eje5);

            angulo6 = options[selectedIndex].value6;
            if (angulo6 < 0)
            {
                eje6 = new Vector3(-1, 0, 0);
            }
            else
            {
                eje6 = new Vector3(1, 0, 0);
            }

            rotacionesDeseadas[5] = Quaternion.AngleAxis(Mathf.Abs(angulo6), eje6);


            // Inicia la rotación gradual para todas las articulaciones
            StartCoroutine(RotacionGradual(rotacionesDeseadas));
        }
        else
        {
            // Imprime un mensaje de error si no se ha seleccionado ninguna opción o si el índice está fuera de rango
            Debug.LogError("No se ha seleccionado ninguna opción o el índice está fuera de rango.");
        }
    }

    public void MoveToPointIndex(int index)
    {
        pointReached = false;
        int selectedIndex = index;
        if (selectedIndex >= 0 && selectedIndex < options.Count)
        {

            Quaternion[] rotacionesDeseadas = new Quaternion[6];
            Vector3[] ejes = new Vector3[6];

            // Obtener los valores de rotación utilizando el script obtenerRotacionScript
            angulo1 = options[selectedIndex].value1;
            if (angulo1 < 0)
            {
                eje1 = new Vector3(0, 0, 1);
            }
            else
            {
                eje1 = new Vector3(0, 0, -1);
            }
            rotacionesDeseadas[0] = Quaternion.AngleAxis(Mathf.Abs(angulo1), eje1);

            angulo2 = options[selectedIndex].value2;
            if (angulo2 < 0)
            {
                eje2 = new Vector3(0, 1, 0);
            }
            else
            {
                eje2 = new Vector3(0, -1, 0);
            }
            rotacionesDeseadas[1] = Quaternion.AngleAxis(Mathf.Abs(angulo2), eje2);

            angulo3 = options[selectedIndex].value3;

            if (angulo3 < 0)
            {
                eje3 = new Vector3(0, 1, 0);
            }
            else
            {
                eje3 = new Vector3(0, -1, 0);
            }
            rotacionesDeseadas[2] = Quaternion.AngleAxis(Mathf.Abs(angulo3), eje3);

            angulo4 = options[selectedIndex].value4;
            if (angulo4 < 0)
            {
                eje4 = new Vector3(-1, 0, 0);
            }
            else
            {
                eje4 = new Vector3(1, 0, 0);
            }
            rotacionesDeseadas[3] = Quaternion.AngleAxis(Mathf.Abs(angulo4), eje4);

            angulo5 = options[selectedIndex].value5;
            if (angulo5 < 0)
            {
                eje5 = new Vector3(0, 1, 0);
            }
            else
            {
                eje5 = new Vector3(0, -1, 0);
            }
            rotacionesDeseadas[4] = Quaternion.AngleAxis(Mathf.Abs(angulo5), eje5);

            angulo6 = options[selectedIndex].value6;
            if (angulo6 < 0)
            {
                eje6 = new Vector3(-1, 0, 0);
            }
            else
            {
                eje6 = new Vector3(1, 0, 0);
            }

            rotacionesDeseadas[5] = Quaternion.AngleAxis(Mathf.Abs(angulo6), eje6);


            // Inicia la rotación gradual para todas las articulaciones
            StartCoroutine(RotacionGradual(rotacionesDeseadas));
        }
        else
        {
            // Imprime un mensaje de error si no se ha seleccionado ninguna opción o si el índice está fuera de rango
            Debug.LogError("No se ha seleccionado ninguna opción o el índice está fuera de rango.");
        }
    }


    IEnumerator RotacionGradual(Quaternion[] rotacionesDeseadas)
    {
        float duracionRotacion = 3.0f; // Duración de la rotación gradual
        float tiempoPasado = 0.0f; // Tiempo transcurrido desde el inicio de la rotación gradual
        float veloc = comunicacionesScript.velocidad;


        if (Limite_velocidad.isOn && Comunicacion_.isOn)
        {
            float distancia = comunicacionesScript.distancia[0];
            // recibo la distancia del scrip comunicacion
            duracionRotacion = distancia / 190;
            


        }
        else
        {
            if (velocidad > 0 && velocidad < 100)
            {
                duracionRotacion = velocidad;
            }
            else
            {
                duracionRotacion = 3.0f;
            }
        }



        Quaternion[] rotacionesIniciales = new Quaternion[6];
        for (int i = 0; i < 6; i++)
        {
            rotacionesIniciales[i] = articulaciones[i].localRotation;
        }

        while (tiempoPasado < duracionRotacion)
        {
            tiempoPasado += Time.deltaTime;
            float t = Mathf.Clamp01(tiempoPasado / duracionRotacion);

            // Interpola gradualmente entre las rotaciones iniciales y las rotaciones deseadas para todas las articulaciones
            for (int i = 0; i < 6; i++)
            {
                articulaciones[i].localRotation = Quaternion.Lerp(rotacionesIniciales[i], rotacionesDeseadas[i], t);
            }

            yield return null; // Espera un frame
        }

        // Asigna las rotaciones deseadas al finalizar la interpolación para asegurar que sean exactas
        for (int i = 0; i < 6; i++)
        {
            articulaciones[i].localRotation = rotacionesDeseadas[i];
        }

        pointReached = true;

    }

    public void GuardarVelocidad()
    {
        // Verificar si el campo de entrada tiene un valor válido
        if (!string.IsNullOrEmpty(campoEntrada.text))
        {
            // Convertir el texto del campo de entrada a un valor flotante
            if (float.TryParse(campoEntrada.text, out float valor))
            {
                // Asignar el valor de la velocidad
                velocidad = valor;
                Debug.Log("Velocidad guardada: " + velocidad);
            }
            else
            {
                Debug.LogWarning("El valor introducido no es válido.");
            }
        }
        else
        {
            Debug.LogWarning("No se ha introducido ningún valor.");

            velocidad = 3;
        }
    }

    public void CopyOptions()
    {
        // Limpiar opciones existentes en el dropdown2
        dropdown2.ClearOptions();

        // Crear una lista de opciones para el dropdown2
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

        // Copiar opciones del dropdown1 al dropdown2
        foreach (DropdownOption option in options)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(option.name));
        }

        // Agregar opciones al dropdown2
        dropdown2.AddOptions(dropdownOptions);
    }

    public bool HasReachedPoint(int index)
    {
        // Devuelve true si el punto ha sido alcanzado
        return pointReached;
    }
}