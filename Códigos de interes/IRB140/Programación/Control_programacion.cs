using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Control_programacion : MonoBehaviour
{
    // Referencia al TMP_Dropdown en tu escena
    public TMP_Dropdown programas;
    public TMP_Dropdown condicion1;
    public TMP_Dropdown condicion2;
    public TMP_Dropdown acciones;
    public TMP_Dropdown ListaPT;
    public TMP_Dropdown listaacciones;

    public Obtener_rotacion obtenerRotacionScript;

    private Control_trayectoria controlTrayectoriaScript;
    public GameObject trayectoriasObject;

    private TMP_DropdownManager controlPuntosScript;
    public GameObject PuntosObject;


    public Toggle Hay_comunicacion;
    public GameObject Comunicacion;

    // Estructura para almacenar una opción del TMP_Dropdown junto con sus coordenadas


    public struct Programas
    {
        public string name;
        public string condition1;
        

        // Agrega otros campos según sea necesario

        public Programas(string _name, string _condition1="")
        {
            name = _name;
            condition1 = _condition1;
            
        }
    }

    public List<Programas> options = new List<Programas>();

    public struct Condicion1
    {
        public string name;
        //public string action;

        // Agrega otros campos según sea necesario

        public Condicion1(string _name)
        {
            name = _name;
            //action = _action;

        }
    }

    public List<Condicion1> optionsc1 = new List<Condicion1>()
    {
        new Condicion1("Repetir X veces"),
        new Condicion1("Inicio con entrada Y"),
        new Condicion1("Inicio con no Y"),
        new Condicion1("Repetir X con Y"),
        new Condicion1("Repetir X con no Y")
    };
    // Opciones iniciales del TMP_Dropdown

  
    public struct Accion
    {
        public string name;
        public string tipo_accion;

        public Accion(string _name, string _tipo_accion)
        {
            name = _name;
            tipo_accion = _tipo_accion;
        }
    }

    public List<Accion> accion = new List<Accion>()
    {
        new Accion("Mover a punto", "punto"),
        new Accion("Mover trayectoria", "trayectoria")
    };


    public struct listaAcciones
    {
        public string name;
        public List<Accion> acciones;
        

        // Agrega otros campos según sea necesario

        public listaAcciones(string _name)
        {
            name = _name;
            acciones = new List<Accion>();
        }
    }

    public List<listaAcciones> listaaccion = new List<listaAcciones>();

    //blic List<Accion> listaAcciones = new List<Accion>();
    public Transform[] articulaciones = new Transform[6]; // Array que contiene todas las articulaciones


    void Start()
    {
        // Inicializar el dropdown de condiciones
        List<TMP_Dropdown.OptionData> conditionOptions = new List<TMP_Dropdown.OptionData>();
        foreach (Condicion1 option in optionsc1)
        {
            conditionOptions.Add(new TMP_Dropdown.OptionData(option.name));
        }
        condicion1.AddOptions(conditionOptions);

        List<TMP_Dropdown.OptionData> actionOptions = new List<TMP_Dropdown.OptionData>();
        foreach (Accion option in accion)
        {
            actionOptions.Add(new TMP_Dropdown.OptionData(option.name));
        }
        acciones.AddOptions(actionOptions);

        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (var option in listaaccion)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(option.name));
        }
        listaacciones.AddOptions(dropdownOptions);

        if (trayectoriasObject != null)
        {
            controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria>();
            if (controlTrayectoriaScript == null)
            {
                Debug.LogError("No se encontró el script Control_trayectoria en el GameObject 'Trayectorias'");
            }
        }
        else
        {
            Debug.LogError("No se asignó el GameObject 'Trayectorias'");
        }

        if (PuntosObject != null)
        {
            controlPuntosScript = PuntosObject.GetComponent<TMP_DropdownManager>();
            if (controlPuntosScript == null)
            {
                Debug.LogError("No se encontró el script Control_trayectoria en el GameObject 'Puntos'");
            }
        }
        else
        {
            Debug.LogError("No se asignó el GameObject 'Puntos'");
        }

        // Añadir listener para manejar la selección de un programa
        programas.onValueChanged.AddListener(delegate { OnProgramSelected(); });
        // Añadir listener para manejar la selección de una condición
        condicion1.onValueChanged.AddListener(delegate { OnConditionSelected(); });

        acciones.onValueChanged.AddListener(delegate { OnActionSelected(); });

        listaacciones.onValueChanged.AddListener(delegate { OnListActionSelectedaccion(); });

        // Inicializar opciones del dropdown de programas
        RefreshDropdownOptions();
    }
    public void AddOption()
    {
        // Genera un nombre para la nueva opción
        string newOption = "Programa " + (programas.options.Count + 1);

        // Crea un nuevo DropdownOption con valores predeterminados
        Programas option = new Programas(newOption);

        // Añade la nueva opción a la lista
        options.Add(option);

        // Actualiza las opciones del TMP_Dropdown
        RefreshDropdownOptions();
    }

    // Método para actualizar las opciones del TMP_Dropdown
    public void RefreshDropdownOptions()
    {
        // Borra todas las opciones existentes en el TMP_Dropdown
        programas.ClearOptions();

        // Agrega las opciones actualizadas al TMP_Dropdown
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (Programas option in options)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(option.name));
        }
        programas.AddOptions(dropdownOptions);

    }

    public void DeleteOption()
    {
        int selectedIndex = programas.value;
        if (selectedIndex >= 0 && selectedIndex < options.Count)
        {
            // Eliminar la opción seleccionada
            options.RemoveAt(selectedIndex);

            // Renombrar las opciones restantes
            for (int i = selectedIndex; i < options.Count; i++)
            {
                // Asignar la nueva opción con el índice actualizado y los datos de la opción anterior
                options[i] = new Programas("Programa " + (i + 1));
            }

            // Actualizar las opciones del TMP_Dropdown
            RefreshDropdownOptions();
        }
        else
        {
            Debug.LogWarning("No se ha seleccionado ninguna opción o el índice está fuera de rango.");
        }
    }

    public void Addlistaaccion()
    {
        string newOption = "Acción " + (listaacciones.options.Count + 1);
        listaAcciones optiones = new listaAcciones(newOption);
        listaaccion.Add(optiones);
        RefreshDropdownaccion();

    }

    public void AddAccionToLista()
    {
        int index = listaacciones.options.Count-1;
        if (index >= 0 && index < listaacciones.options.Count)
        {
            // Agregar una nueva acción a la lista de acciones en el índice especificado
            //string newActionName = "Nueva Acción " + (listaAcciones[index].acciones.Count + 1);
           // Accion newAction = new Accion(newActionName, ""); // Deja el tipo de acción vacío inicialmente
            //listaAcciones[index].acciones.Add(newAction);
            // Actualizar la interfaz de usuario si es necesario
            RefreshDropdownaccion();
        }
    }

    public void RefreshDropdownaccion()
    {
        listaacciones.ClearOptions();

        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (var option in listaaccion)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(option.name));
        }
        listaacciones.AddOptions(dropdownOptions);
    }

    private void OnListActionSelectedaccion()
    {
        // Actualiza el dropdown correspondiente según la acción seleccionada en listaacciones
        OnActionSelected();
    }

    private void OnProgramSelected()
    {
        int selectedIndex = programas.value;
        if (selectedIndex >= 0 && selectedIndex < options.Count)
        {
            string selectedCondition = options[selectedIndex].condition1;
            int conditionIndex = optionsc1.FindIndex(c => c.name == selectedCondition);
            if (conditionIndex >= 0)
            {
                condicion1.value = conditionIndex;
            }
            else
            {
                condicion1.value = 0; // Set default if not found
            }
        }
    }

    private void OnConditionSelected()
    {
        int selectedProgramIndex = programas.value;
        if (selectedProgramIndex >= 0 && selectedProgramIndex < options.Count)
        {
            string selectedCondition = condicion1.options[condicion1.value].text;
            options[selectedProgramIndex] = new Programas(options[selectedProgramIndex].name, selectedCondition);
        }
    }

    void OnActionSelected()
    {
        // Borra las opciones existentes en el segundo dropdown
        ListaPT.ClearOptions();

        // Obtiene la acción seleccionada
        int selectedIndex = acciones.value;
        //if (selectedIndex < 0 || selectedIndex >= acciones.Count) return;

        string tipoAccion = accion[selectedIndex].tipo_accion;

        // Añade las opciones correspondientes basadas en el tipo de acción seleccionado
        List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();
        if (tipoAccion == "punto")
        {
            foreach (var puntos in controlPuntosScript.options)
            {
                optionDataList.Add(new TMP_Dropdown.OptionData(puntos.name));
            }
        }
        else if (tipoAccion == "trayectoria")
        {
            foreach (var trayectoria in controlTrayectoriaScript.options)
            {
                optionDataList.Add(new TMP_Dropdown.OptionData(trayectoria.name));
            }
        }

        ListaPT.AddOptions(optionDataList);
    }

    private void OnListActionSelected()
    {
        // Actualiza el dropdown correspondiente según la acción seleccionada en listaacciones
        OnActionSelected();
    }

    public void CrearCodigo()
    {
        int selectedIndex = programas.value;
        if (selectedIndex >= 0 && selectedIndex < options.Count)
        {
            string selectedCondition = options[selectedIndex].condition1;
            int conditionIndex = optionsc1.FindIndex(c => c.name == selectedCondition);
            if (conditionIndex >= 0)
            {
                condicion1.value = conditionIndex;
            }
            else
            {
                condicion1.value = 0; // Set default if not found
            }
        }
    }
}
