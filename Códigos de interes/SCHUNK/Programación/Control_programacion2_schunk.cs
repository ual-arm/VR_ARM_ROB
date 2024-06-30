using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.Http;
using System.Threading;
using UnityEditor.VersionControl;
using System.IO;
using static Control_programacion2_schunk;

public class Control_programacion2_schunk : MonoBehaviour
{
    // Referencia al TMP_Dropdown en tu escena

    public TMP_Dropdown programa;
    public TMP_Dropdown valor_condicion;
    public TMP_Dropdown condicion;
    public TMP_Dropdown numeroacciones;
    public TMP_Dropdown tipoaccion;
    public TMP_Dropdown numerotipoaccion;

    public TMP_Dropdown Valor_y;
    public TMP_InputField Valor_x;

    public Obtener_rotacion_schunk obtenerRotacionScript;

    private Control_trayectoria_schunk MoverTrayectoriaScript;
    private Control_trayectoria_schunk controlTrayectoriaScript;
    public GameObject trayectoriasObject;

    private Comunicacion_schunk comunicacionScript;
    public GameObject comunicacionObject;

    public creacion_texto_programa crear_texto;
    public creacion_texto_programa borrar_texto;
    public GameObject textoObject;
    public GameObject textoObject2;

    private TMP_DropdownManager_schunk MoverPuntosScript;
    private TMP_DropdownManager_schunk controlPuntosScript;
    public GameObject PuntosObject;


    public Toggle Hay_comunicacion;
    public GameObject Comunicacion;

    //public string filePrograma = "Assets/YourFolder/YourFile.txt";

    private string nombreArchivoprograma = "Programas_schunk.txt";

    string directorioProyecto;
    string rutaCompletaprograma;

    public List<Toggle> entradas;
    public List<Toggle> entradas2;
    public List<Toggle> salidas;
    public List<Toggle> salidas2;


    private bool shouldContinue = true;
    // Estructura para almacenar una opción del TMP_Dropdown junto con sus coordenadas


    public struct Condicion
    {
        public string name;
        public string valorCondicion;
        public List<Accion> acciones;
        public float x;
        public int y;

        // Añadir campos para almacenar los valores seleccionados de los dropdowns
        public int selectedProgramaIndex;
        public int selectedValorCondicionIndex;
        public int selectedCondicionIndex;
        public int selectedNumeroAccionesIndex;
        public int selectedTipoAccionIndex;
        public int selectedNumeroTipoAccionIndex;

        public Condicion(string _name, string _valorCondicion)
        {
            name = _name;
            valorCondicion = _valorCondicion;
            acciones = new List<Accion>();

            // Inicializar los índices de los dropdowns
            selectedProgramaIndex = 0;
            selectedValorCondicionIndex = 0;
            selectedCondicionIndex = 0;
            selectedNumeroAccionesIndex = 0;
            selectedTipoAccionIndex = 0;
            selectedNumeroTipoAccionIndex = 0;
            x = 1;
            y = 1;
        }
    }

    public List<Condicion> listaCondiciones = new List<Condicion>();

    public class Accion
    {
        public string name;
        public string tipo_accion;
        public int numero_accion;

        public Accion(string _name, string _tipo_accion, int _numero_accion)
        {
            name = _name;
            tipo_accion = _tipo_accion;
            numero_accion = _numero_accion;
        }
    }



    public struct listaAcciones
    {
        public string name;
        public List<Accion> accion;
        public int selectedTipoAccionIndex;
        public int selectedNumeroTipoAccionIndex;
        public string nombreTipoAccion;
        public string nombreNumeroTipoAccion;
        // Agrega otros campos según sea necesario

        public listaAcciones(string _name)
        {
            name = _name;
            accion = new List<Accion>();
            selectedTipoAccionIndex = 0;
            selectedNumeroTipoAccionIndex = 0;
            nombreTipoAccion = "";
            nombreNumeroTipoAccion = "";
        }
    }

    public List<listaAcciones> listaaccion = new List<listaAcciones>();

    public List<string> opcionescondicion = new List<string>()
    {
        new string("Repetir X veces"),
        new string("Inicio con entrada Y"),
        new string("Inicio con no Y"),
        new string("Repetir X con Y"),
        new string("Repetir X con no Y")
    };

    public void Start()
    {
        directorioProyecto = Directory.GetParent(Application.dataPath).FullName;

        rutaCompletaprograma = Path.Combine(directorioProyecto, nombreArchivoprograma);
        if (!File.Exists(rutaCompletaprograma))
        {
            Debug.LogError("El archivo no existe: ");
            return;
        }
    }

    public void iniciar()
    {

        if (trayectoriasObject != null)
        {
            controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_schunk>();
            if (controlTrayectoriaScript == null)
            {
                Debug.LogError("No se encontró el script Control_trayectoria en el GameObject 'Trayectorias'");
            }
        }
        else
        {
            Debug.LogError("No se asignó el GameObject 'Trayectorias'");
        }

        if (trayectoriasObject != null)
        {
            MoverTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_schunk>();
            if (MoverTrayectoriaScript == null)
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
            controlPuntosScript = PuntosObject.GetComponent<TMP_DropdownManager_schunk>();
            if (controlPuntosScript == null)
            {
                Debug.LogError("No se encontró el script TMP_DropdownManager en el GameObject 'Puntos'");
            }
        }
        else
        {
            Debug.LogError("No se asignó el GameObject 'Puntos'");
        }

        if (PuntosObject != null)
        {

            MoverPuntosScript = PuntosObject.GetComponent<TMP_DropdownManager_schunk>();
            if (MoverPuntosScript == null)
            {
                Debug.LogError("No se encontró el script Control_trayectoria en el GameObject 'Trayectorias'");
            }
        }
        else
        {
            Debug.LogError("No se asignó el GameObject 'Trayectorias'");
        }

        crear_texto = textoObject.GetComponent<creacion_texto_programa>();
        //crear_texto = textoObject2.GetComponent<creacion_texto_programa>();
        borrar_texto = textoObject.GetComponent<creacion_texto_programa>();
        // borrar_texto = textoObject2.GetComponent<creacion_texto_programa>();
        comunicacionScript = comunicacionObject.GetComponent<Comunicacion_schunk>();

        //AddOptionsToDropdown(condicion, opcionescondicion);
        //valor_condicion.onValueChanged.AddListener(delegate { OnNumeroCondicionSelected(); });
        //numeroacciones.onValueChanged.AddListener(delegate { OnNumeroAccionesSelected(); });
        //tipoaccion.onValueChanged.AddListener(delegate { OnTipoAccionSelected(); });
        //numerotipoaccion.onValueChanged.AddListener(delegate { OnNumeroTipoAccionSelected(); });

        //RefreshTipoAccionOptions();

        //RefreshDropdownOptions();
    }

    // Esta parte se usa para añadir las opciones a las condiciones y se ejecuta solo al iniciar el programa
    void AddOptionsToDropdown(TMP_Dropdown dropdown, List<string> opciones)
    {
        dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (string opcion in opciones)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(opcion));
        }
        dropdown.AddOptions(dropdownOptions);
    }


    // Esta parte añade puntos nuevos a la lista de acciones perteneciente a la condicion seleccionada

    public void AddOptionlistaacciones()
    {

        int selectedIndex = valor_condicion.value;
        if (selectedIndex >= 0 && selectedIndex < listaCondiciones.Count)
        {
            Condicion selectedCondition = listaCondiciones[selectedIndex];
            // Crear una nueva acción y añadirla a la lista de acciones de la condición seleccionada
            string newActionName = "Accion " + (selectedCondition.acciones.Count + 1);
            Accion newAction = new Accion(newActionName, "Mover a punto", 0);
            selectedCondition.acciones.Add(newAction);
            listaCondiciones[selectedIndex] = selectedCondition;
            // Actualizar las opciones de acciones disponibles para la condición seleccionada
            RefreshDropdownOptions(numeroacciones, selectedCondition.acciones);
        }


    }
    // Aqui se se esta actualizando las opciones que aparecen en el dropdown de acciones para añadir la nueva, ademas llama
    // a refrescar tipo acciones, de esta forma crea dos acciones posibles y dentro de cada accion posible la lista de puntos o trauye
    void RefreshDropdownOptions(TMP_Dropdown dropdown, List<Accion> acciones)
    {
        dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (Accion accion in acciones)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(accion.name));
        }
        dropdown.AddOptions(dropdownOptions);

        //RefreshTipoAccionOptions();
        OnTipoAccionSelected();
    }

    // Método para actualizar las opciones del TMP_Dropdown

    // Con este metodo se cargan las trayectorias y puntos que podremos usar en el codigo
    void RefreshTipoAccionOptions()
    {
        tipoaccion.ClearOptions();
        List<Accion> accionesDisponibles = new List<Accion>()
        {
            new Accion("Mover a punto", "punto",0),
            new Accion("Mover trayectoria", "trayectoria",0),
            new Accion("Activar salida", "salida",0)
        };

        List<TMP_Dropdown.OptionData> actionOptions = new List<TMP_Dropdown.OptionData>();
        foreach (Accion accion in accionesDisponibles)
        {
            actionOptions.Add(new TMP_Dropdown.OptionData(accion.name));
        }
        tipoaccion.AddOptions(actionOptions);

        OnTipoAccionSelected();
    }
    public void OnTipoAccionSelected()
    {

        numerotipoaccion.ClearOptions();
        int selectedIndex = tipoaccion.value;
        if (selectedIndex >= 0)
        {
            string selectedTipoAccion = tipoaccion.options[selectedIndex].text;
            List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();
            if (selectedTipoAccion == "Mover a punto")
            {
                foreach (var puntos in controlPuntosScript.options)
                {
                    optionDataList.Add(new TMP_Dropdown.OptionData(puntos.name));
                }

            }
            if (selectedTipoAccion == "Mover trayectoria")
            {
                foreach (var trayectoria in controlTrayectoriaScript.options)
                {
                    optionDataList.Add(new TMP_Dropdown.OptionData(trayectoria.name));
                }
            }
            if (selectedTipoAccion == "Activar salida")
            {

                optionDataList.Add(new TMP_Dropdown.OptionData("Salida 1"));
                optionDataList.Add(new TMP_Dropdown.OptionData("Salida 2"));
                optionDataList.Add(new TMP_Dropdown.OptionData("Salida 3"));
                optionDataList.Add(new TMP_Dropdown.OptionData("Salida 4"));
                optionDataList.Add(new TMP_Dropdown.OptionData("Salida 5"));

            }
            numerotipoaccion.AddOptions(optionDataList);
        }

    }

    public void GuardarValorX()
    {
        int selectedIndex = valor_condicion.value;
        Condicion selectedCondition = listaCondiciones[selectedIndex];
        // Verificar si el campo de entrada tiene un valor válido
        if (!string.IsNullOrEmpty(Valor_x.text))
        {

            // Convertir el texto del campo de entrada a un valor flotante
            if (float.TryParse(Valor_x.text, out float valor))
            {

                selectedCondition.x = valor;
                listaCondiciones[selectedIndex] = selectedCondition;

            }
        }
        else
        {

            selectedCondition.x = 1;
            listaCondiciones[selectedIndex] = selectedCondition;
        }
    }

    // Esta funcion y la siguiente ahora mismo no estan actualizadas

    public void Mostrarprograma()
    {
        int posicion;
        posicion = 0;
        borrar_texto.BorrarTextos();

        for (int i = 0; i < valor_condicion.options.Count; i++)
        {
            string condicionSeleccionada;
            Condicion selectedCondition = listaCondiciones[i];
            //selectedCondition.selectedCondicionIndex = i;

            int condicionSeleccionadaIndex = selectedCondition.selectedCondicionIndex;
            float x = selectedCondition.x;
            int y = selectedCondition.y;

            // Necesito una funcion que dado la posicion, el valor condicion, x e y escriba la condicion.
            crear_texto.escribir_condiciones(condicionSeleccionadaIndex, x, y, posicion);
            posicion = posicion + 1;

            for (int j = 0; j < selectedCondition.acciones.Count; j++)
            {
                string accion = selectedCondition.acciones[j].tipo_accion;
                int numero_accion = selectedCondition.acciones[j].numero_accion;

                // Necesito una funcion que dado la posicion, el valor condicion, x e y escriba la condicion.
                crear_texto.escribir_acciones(numero_accion, accion, posicion);
                posicion = posicion + 1;
            }

        }
    }


    // ESta funcion agrega y elimina nuevas opciones a las condiciones
    public void AgregarOpcionCondicion()
    {
        // Obtener el número de opciones actuales en el dropdown
        int numOpciones = valor_condicion.options.Count;

        // Crear una nueva opción con el formato C1, C2, etc.
        string nuevaOpcion = "C" + (numOpciones + 1);
        Condicion option = new Condicion(nuevaOpcion, "");
        listaCondiciones.Add(option);
        // Agregar la nueva opción al dropdown
        valor_condicion.options.Add(new TMP_Dropdown.OptionData(nuevaOpcion));

        // Refrescar el dropdown para que muestre la nueva opción
        valor_condicion.RefreshShownValue();

    }

    public void EliminarOpcionCondicion()
    {
        int indiceSeleccionado = valor_condicion.value;

        // Verificar que hay al menos una opción en el dropdown y que el índice es válido


        // Verificar que hay al menos una opción en el dropdown y que el índice es válido
        if (valor_condicion.options.Count > 0 && indiceSeleccionado < listaCondiciones.Count)
        {

            Condicion selectedCondicion = listaCondiciones[indiceSeleccionado];

            for (int i = selectedCondicion.acciones.Count - 1; i >= 0; i--)
            {
                selectedCondicion.acciones.RemoveAt(i);
                numeroacciones.options.RemoveAt(i);
                listaCondiciones[indiceSeleccionado] = selectedCondicion;
            }

            // listaCondiciones[indiceSeleccionado] = selectedCondicion;

            // Eliminar la condición correspondiente de la lista
            listaCondiciones.RemoveAt(indiceSeleccionado);

            // Eliminar la opción seleccionada del dropdown
            valor_condicion.options.RemoveAt(indiceSeleccionado);

            // Actualizar los nombres y los índices de las condiciones restantes
            //for (int i = indiceSeleccionado; i < listaCondiciones.Count; i++)
            //{
            //    Condicion condicion = listaCondiciones[i];
            //    condicion.name = "C" + (i + 1); // Actualizar el nombre de la condición
            //    condicion.selectedCondicionIndex = i; // Actualizar el índice de la condición
            //    listaCondiciones[i] = condicion;
            //}


            for (int i = indiceSeleccionado; i < listaCondiciones.Count; i++)
            {

                Condicion condicion = listaCondiciones[i];
                condicion.name = "C" + (i + 1); // Actualizar el nombre de la condición
                //condicion.valorCondicion = ;
                //condicion.selectedCondicionIndex = i; // Actualizar el índice de la condición
                listaCondiciones[i] = condicion;
            }

            // Actualizar los nombres de las opciones en el dropdown
            valor_condicion.ClearOptions();
            List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < listaCondiciones.Count; i++)
            {
                dropdownOptions.Add(new TMP_Dropdown.OptionData(listaCondiciones[i].name));
            }
            valor_condicion.AddOptions(dropdownOptions);

            // Refrescar el dropdown para que muestre las opciones actualizadas
            valor_condicion.RefreshShownValue();

            // Borrar las acciones relacionadas con la condición eliminada
            //foreach (var condicion in listaCondiciones)
            //{
            //    condicion.acciones.Clear();
            //}
        }
    }

    // ESta funcion lo que hace es que si seleciono una condicion le asigna a los dropdown los valores que tienen asignados
    // En este caso se esta actualizando el valor de condicion y la lista de acciones.
    public void OnNumeroCondicionSelected()
    {

        int selectedIndex = valor_condicion.value;
        if (selectedIndex >= 0 && selectedIndex < listaCondiciones.Count)
        {
            Condicion selectedCondicion = listaCondiciones[selectedIndex];
            condicion.value = selectedCondicion.selectedCondicionIndex;
            Valor_x.text = selectedCondicion.x.ToString();
            Valor_y.value = selectedCondicion.y;

            string valorCondicion = selectedCondicion.valorCondicion;


            //SetDropdownValue(valor_condicion, valorCondicion);
            //numerotipoaccion.value = selectedCondicion.selectedNumeroTipoAccionIndex;
            //tipoaccion.value = selectedCondicion.selectedTipoAccionIndex;
            RefreshDropdownCondicion(selectedCondicion.acciones);
        }
    }



    public void OnNumeroAccionesSelected()
    {
        int selectedIndex = valor_condicion.value;
        int selectedIndex2 = numeroacciones.value;
        Condicion selectedCondition = listaCondiciones[selectedIndex];
        if (selectedIndex >= 0 && selectedIndex < listaCondiciones.Count && selectedIndex2 < selectedCondition.acciones.Count && selectedIndex2 >= 0)
        {
            // Obtener la condición seleccionada

            if (selectedCondition.acciones[selectedIndex2].tipo_accion == "Mover a punto")
            {
                tipoaccion.value = 0;
            }
            if (selectedCondition.acciones[selectedIndex2].tipo_accion == "Mover trayectoria")
            {
                tipoaccion.value = 1;
            }
            if (selectedCondition.acciones[selectedIndex2].tipo_accion == "Activar salida")
            {
                tipoaccion.value = 2;
            }

            OnTipoAccionSelected();
            numerotipoaccion.value = selectedCondition.acciones[selectedIndex2].numero_accion;
        }


    }

    public void OnNumeroTipoAccionSelected()
    {
        // Update the selected action with the new selection
        UpdateSelectedAction();
    }

    public void UpdateSelectedCondicion()
    {
        int selectedIndex = valor_condicion.value;
        int selectedIndex2 = numeroacciones.value;
        int selectedIndex3 = tipoaccion.value;


        if (selectedIndex >= 0 && selectedIndex < listaCondiciones.Count)
        {
            Condicion selectedCondition = listaCondiciones[selectedIndex];
            selectedCondition.selectedCondicionIndex = condicion.value;
            listaCondiciones[selectedIndex] = selectedCondition;
            //RefreshDropdownCondicion(selectedCondition.acciones);
        }

    }

    public void UpdateSelectedValorY()
    {
        int selectedIndex = valor_condicion.value;
        int selectedIndex2 = numeroacciones.value;
        int selectedIndex3 = tipoaccion.value;


        if (selectedIndex >= 0 && selectedIndex < Valor_y.options.Count)
        {
            Condicion selectedCondition = listaCondiciones[selectedIndex];
            selectedCondition.y = Valor_y.value;
            listaCondiciones[selectedIndex] = selectedCondition;
            //RefreshDropdownCondicion(selectedCondition.acciones);
        }

    }

    public void UpdateSelectedAction()
    {
        int selectedIndex = valor_condicion.value;
        int selectedIndex2 = numeroacciones.value;
        int selectedIndex3 = tipoaccion.value;

        Condicion selectedCondition = listaCondiciones[selectedIndex];
        if (selectedIndex2 >= 0 && selectedIndex2 < numeroacciones.options.Count)
        {
            // Modificar el tipo_accion
            selectedCondition.acciones[selectedIndex2].tipo_accion = tipoaccion.options[tipoaccion.value].text;
        }
        //Debug.Log("Casi he entradossss: " + tipoaccion.options[tipoaccion.value].text);

        //RefreshDropdownCondicion(selectedCondition.acciones);
        OnTipoAccionSelected();
        listaCondiciones[selectedIndex] = selectedCondition;
    }

    public void UpdateSelectedNumeroAction()
    {
        int selectedIndex = valor_condicion.value;
        int selectedIndex2 = numeroacciones.value;
        int selectedIndex3 = tipoaccion.value;


        // Modificar el tipo_accion

        Condicion selectedCondition = listaCondiciones[selectedIndex];
        if (selectedIndex3 >= 0 && selectedIndex3 < tipoaccion.options.Count)
        {
            selectedCondition.acciones[selectedIndex2].numero_accion = numerotipoaccion.value;
        }

        listaCondiciones[selectedIndex] = selectedCondition;
        //RefreshDropdownCondicion(selectedCondition.acciones);
    }


    void RefreshDropdownCondicion(List<Accion> acciones)
    {
        numeroacciones.ClearOptions();
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (Accion accion in acciones)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(accion.name));
        }
        numeroacciones.AddOptions(dropdownOptions);

        //RefreshTipoAccionOptions();
    }


    public void WritePrograma()
    {

        if (!File.Exists(rutaCompletaprograma))
        {
            File.Create(rutaCompletaprograma).Close();
        }

        using (StreamWriter writer = new StreamWriter(rutaCompletaprograma))
        {
            //writer.WriteLine("Trayectorias");
            for (int i = 0; i < valor_condicion.options.Count; i++)
            {
                Condicion selectedCondition = listaCondiciones[i];
                selectedCondition.selectedCondicionIndex = i;
                writer.WriteLine("Condicion");
                writer.WriteLine(selectedCondition.selectedCondicionIndex);
                writer.WriteLine(selectedCondition.x);
                writer.WriteLine(selectedCondition.y);

                for (int j = 0; j < selectedCondition.acciones.Count; j++)
                {
                    string accion = selectedCondition.acciones[j].tipo_accion;
                    int numero_accion = selectedCondition.acciones[j].numero_accion;
                    writer.WriteLine("Accion");
                    writer.WriteLine(accion);
                    writer.WriteLine(numero_accion);

                }

            }

        }
        // Debug.Log("Datos de trayectorias guardados en el archivo: " + fileTrajectory);
    }

    public void LoadPrograma()
    {
        AddOptionsToDropdown(condicion, opcionescondicion);
        RefreshTipoAccionOptionse();
        iniciar();

        if (File.Exists(rutaCompletaprograma))
        {

            string[] lines = File.ReadAllLines(rutaCompletaprograma);
            int numberOfLines = lines.Length;
            int i = 0;
            int j = 0;
            while (j < numberOfLines)
            {
                if (j < numberOfLines && lines[j] == "Condicion")
                {
                    //Debug.Log("Estoy cargando condicion");
                    int numOpciones = i;
                    string nuevaOpcion = "C" + (i + 1);
                    Condicion option = new Condicion(nuevaOpcion, "");
                    listaCondiciones.Add(option);
                    valor_condicion.options.Add(new TMP_Dropdown.OptionData(nuevaOpcion));
                    valor_condicion.RefreshShownValue();

                    j = j + 1;
                    int condicion_num;
                    int.TryParse(lines[j], out condicion_num);
                    Condicion selectedCondition = listaCondiciones[i];
                    selectedCondition.selectedCondicionIndex = condicion_num;
                    listaCondiciones[i] = selectedCondition;

                    j = j + 1;
                    int x_num;
                    int.TryParse(lines[j], out x_num);
                    Condicion selectedCondition2 = listaCondiciones[i];
                    selectedCondition2.x = x_num;
                    listaCondiciones[i] = selectedCondition2;

                    j = j + 1;
                    int y_num;
                    int.TryParse(lines[j], out y_num);
                    Condicion selectedCondition3 = listaCondiciones[i];
                    selectedCondition3.y = y_num;
                    listaCondiciones[i] = selectedCondition3;

                    i = i + 1;
                    j = j + 1;

                    //Debug.Log("El j es: " + j);
                }
                if (j < numberOfLines && lines[j] == "Accion")
                {
                    //Debug.Log("Estoy cargando accion");
                    string tipo;
                    int numero;

                    j = j + 1;
                    tipo = lines[j];
                    j = j + 1;
                    int.TryParse(lines[j], out numero);

                    Condicion selectedCondition = listaCondiciones[i - 1];
                    string newActionName = "Accion " + (selectedCondition.acciones.Count + 1);
                    Accion newAction = new Accion(newActionName, tipo, numero);
                    selectedCondition.acciones.Add(newAction);
                    listaCondiciones[i - 1] = selectedCondition;
                    // Actualizar las opciones de acciones disponibles para la condición seleccionada
                    RefreshDropdownOptionse(numeroacciones, selectedCondition.acciones);

                    List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();
                    if (tipo == "Mover a punto")
                    {
                        foreach (var puntos in controlPuntosScript.options)
                        {
                            optionDataList.Add(new TMP_Dropdown.OptionData(puntos.name));
                        }

                    }
                    if (tipo == "Mover trayectoria")
                    {
                        foreach (var trayectoria in controlTrayectoriaScript.options)
                        {
                            optionDataList.Add(new TMP_Dropdown.OptionData(trayectoria.name));
                        }
                    }
                    if (tipo == "Activar salida")
                    {

                        optionDataList.Add(new TMP_Dropdown.OptionData("Salida 1"));
                        optionDataList.Add(new TMP_Dropdown.OptionData("Salida 2"));
                        optionDataList.Add(new TMP_Dropdown.OptionData("Salida 3"));
                        optionDataList.Add(new TMP_Dropdown.OptionData("Salida 4"));
                        optionDataList.Add(new TMP_Dropdown.OptionData("Salida 5"));

                    }
                    numerotipoaccion.AddOptions(optionDataList);


                    j = j + 1;

                }

            }

        }
        else
        {
            Debug.LogWarning("El archivo de trayectorias no existe.");
        }

        //OnNumeroCondicionSelected();
        //GuardarValorX();
        //OnNumeroAccionesSelected();
    }



    void RefreshDropdownOptionse(TMP_Dropdown dropdown, List<Accion> acciones)
    {
        dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (Accion accion in acciones)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(accion.name));
        }
        dropdown.AddOptions(dropdownOptions);

        //RefreshTipoAccionOptions();
        //OnTipoAccionSelected();
    }

    void RefreshTipoAccionOptionse()
    {
        tipoaccion.ClearOptions();
        List<Accion> accionesDisponibles = new List<Accion>()
        {
            new Accion("Mover a punto", "punto",0),
            new Accion("Mover trayectoria", "trayectoria",0),
            new Accion("Activar salida", "salida",0)
        };

        List<TMP_Dropdown.OptionData> actionOptions = new List<TMP_Dropdown.OptionData>();
        foreach (Accion accion in accionesDisponibles)
        {
            actionOptions.Add(new TMP_Dropdown.OptionData(accion.name));
        }
        tipoaccion.AddOptions(actionOptions);

        //OnTipoAccionSelectede();
    }


    public void OnTipoAccionSelectede()
    {

        numerotipoaccion.ClearOptions();
        int selectedIndex = tipoaccion.value;
        if (selectedIndex >= 0)
        {
            string selectedTipoAccion = tipoaccion.options[selectedIndex].text;
            List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();
            if (selectedTipoAccion == "Mover a punto")
            {
                foreach (var puntos in controlPuntosScript.options)
                {
                    optionDataList.Add(new TMP_Dropdown.OptionData(puntos.name));
                }

            }
            else if (selectedTipoAccion == "Mover trayectoria")
            {
                foreach (var trayectoria in controlTrayectoriaScript.options)
                {
                    optionDataList.Add(new TMP_Dropdown.OptionData(trayectoria.name));
                }
            }
            if (selectedTipoAccion == "Activar salida")
            {

                optionDataList.Add(new TMP_Dropdown.OptionData("Salida 1"));
                optionDataList.Add(new TMP_Dropdown.OptionData("Salida 2"));
                optionDataList.Add(new TMP_Dropdown.OptionData("Salida 3"));
                optionDataList.Add(new TMP_Dropdown.OptionData("Salida 4"));
                optionDataList.Add(new TMP_Dropdown.OptionData("Salida 5"));

            }
            numerotipoaccion.AddOptions(optionDataList);
        }

    }
    public void Ejecutar_programa()
    {
        StartCoroutine(Ejecutar_programa2());
    }
    public IEnumerator Ejecutar_programa2()
    {
        int condicion;
        float valorx;
        int valory;

        if (comunicacionScript.ActiveC == true)
        {
            //Debug.Log("EStoy aqui");
            //comunicacionScript.Envio_programa(listaCondiciones);
            yield return StartCoroutine(comunicacionScript.Envio_programa(listaCondiciones));


        }

        int completo = 0;
        for (int i = 0; i < valor_condicion.options.Count;)
        {
            completo = 0;
            shouldContinue = false;

            Condicion selectedCondition = listaCondiciones[i];
            condicion = selectedCondition.selectedCondicionIndex;
            valorx = selectedCondition.x;
            valory = selectedCondition.y;

            yield return StartCoroutine(comunicacionScript.comprobar_entradas(entradas));


            switch (condicion)
            {
                case 0:
                    for (float j = 0; j < valorx; j++)
                    {

                        for (int q = 0; q < selectedCondition.acciones.Count; q++)
                        {
                            if (selectedCondition.acciones[q].tipo_accion == "Mover a punto")
                            {
                                yield return StartCoroutine(MoverPunto(selectedCondition.acciones[q].numero_accion));

                            }
                            if (selectedCondition.acciones[q].tipo_accion == "Mover trayectoria")
                            {
                                yield return StartCoroutine(MoverTrayectoriaPunto(selectedCondition.acciones[q].numero_accion));
                                yield return StartCoroutine(MoverTrayectoria(selectedCondition.acciones[q].numero_accion));
                            }
                            if (selectedCondition.acciones[q].tipo_accion == "Activar salida")
                            {
                                salidas[selectedCondition.acciones[q].numero_accion].isOn = true;

                            }

                        }
                    }

                    completo += 1;


                    break;
                case 1:
                    StartCoroutine(EsperarEntrada(valory, selectedCondition));
                    
                    break;
                case 2:
                    StartCoroutine(EsperarNoEntrada(valory, selectedCondition));
                    i = i + 1;
                    break;
                case 3:
                    StartCoroutine(EsperarEntradaX(valorx, valory, selectedCondition));
                    i = i + 1;
                    break;
                case 4:
                    StartCoroutine(EsperarNoEntradaX(valorx, valory, selectedCondition));
                    i = i + 1;
                    break;
                default:

                    break;
            }

           if(shouldContinue = true)
            {
                i = i + 1;
            }
            yield return null;

        }

    }

    private IEnumerator MoverPunto(int puntoIndex)
    {
        MoverPuntosScript.MoveToPointIndex(puntoIndex);
        // Supongamos que MoveToPointIndex es asíncrono y puedes verificar cuando ha terminado
        while (!MoverPuntosScript.HasReachedPoint(puntoIndex)) // Verifica si ha terminado
        {
            yield return null; // Espera al siguiente frame y vuelve a comprobar
        }
        shouldContinue = true;
    }

    private IEnumerator MoverTrayectoriaPunto(int trayectoriaIndex)
    {
        MoverTrayectoriaScript.MoverPuntoIndex(trayectoriaIndex);

        // Supongamos que MoverTrayectoriaIndex es asíncrono y puedes verificar cuando ha terminado
        while (!MoverTrayectoriaScript.HasCompletedTrajectoryPoint(trayectoriaIndex)) // Verifica si ha terminado
        {
            yield return null; // Espera al siguiente frame y vuelve a comprobar
        }
        shouldContinue = true;
    }
    private IEnumerator MoverTrayectoria(int trayectoriaIndex)
    {

        MoverTrayectoriaScript.MoverTrayectoriaIndex(trayectoriaIndex);
        // Supongamos que MoverTrayectoriaIndex es asíncrono y puedes verificar cuando ha terminado
        while (!MoverTrayectoriaScript.HasCompletedTrajectory(trayectoriaIndex)) // Verifica si ha terminado
        {
            yield return null; // Espera al siguiente frame y vuelve a comprobar
        }
        shouldContinue = true;
    }

    IEnumerator EsperarEntrada(int valory, Condicion selectedCondition)
    {
        yield return new WaitUntil(() => entradas[valory].isOn);
        yield return StartCoroutine(comunicacionScript.comprobar_entradas(entradas));

        //Condicion selectedCondition = listaCondiciones[i];
        for (int q = 0; q < selectedCondition.acciones.Count; q++)
        {
            if (selectedCondition.acciones[q].tipo_accion == "Mover a punto")
            {
                yield return StartCoroutine(MoverPunto(selectedCondition.acciones[q].numero_accion));

            }
            if (selectedCondition.acciones[q].tipo_accion == "Mover trayectoria")
            {
                yield return StartCoroutine(MoverTrayectoriaPunto(selectedCondition.acciones[q].numero_accion));
                yield return StartCoroutine(MoverTrayectoria(selectedCondition.acciones[q].numero_accion));

            }
            if (selectedCondition.acciones[q].tipo_accion == "Activar salida")
            {
                salidas[selectedCondition.acciones[q].numero_accion].isOn = true;


            }

        }
        shouldContinue = true;

    }

    IEnumerator EsperarNoEntrada(int valory, Condicion selectedCondition)
    {
        yield return new WaitUntil(() => !entradas[valory].isOn);
        yield return StartCoroutine(comunicacionScript.comprobar_entradas(entradas));
        //yield return new WaitUntil(() => !entradas[valory].isOn || !entradas2[valory].isOn);

        //Condicion selectedCondition = listaCondiciones[i];
        for (int q = 0; q < selectedCondition.acciones.Count; q++)
        {
            if (selectedCondition.acciones[q].tipo_accion == "Mover a punto")
            {
                yield return StartCoroutine(MoverPunto(selectedCondition.acciones[q].numero_accion));

            }
            if (selectedCondition.acciones[q].tipo_accion == "Mover trayectoria")
            {
                yield return StartCoroutine(MoverTrayectoriaPunto(selectedCondition.acciones[q].numero_accion));
                yield return StartCoroutine(MoverTrayectoria(selectedCondition.acciones[q].numero_accion));

            }
            if (selectedCondition.acciones[q].tipo_accion == "Activar salida")
            {
                salidas[selectedCondition.acciones[q].numero_accion].isOn = true;


            }

        }
        shouldContinue = true;

    }

    IEnumerator EsperarEntradaX(float x, int valory, Condicion selectedCondition)
    {
        yield return new WaitUntil(() => entradas[valory].isOn);
        yield return StartCoroutine(comunicacionScript.comprobar_entradas(entradas));
        //yield return new WaitUntil(() => entradas[valory].isOn || entradas2[valory].isOn);

        //Condicion selectedCondition = listaCondiciones[i];
        for (float j = 0; j < x; j++)
        {
            for (int q = 0; q < selectedCondition.acciones.Count; q++)
            {
                if (selectedCondition.acciones[q].tipo_accion == "Mover a punto")
                {
                    yield return StartCoroutine(MoverPunto(selectedCondition.acciones[q].numero_accion));

                }
                if (selectedCondition.acciones[q].tipo_accion == "Mover trayectoria")
                {
                    yield return StartCoroutine(MoverTrayectoriaPunto(selectedCondition.acciones[q].numero_accion));
                    yield return StartCoroutine(MoverTrayectoria(selectedCondition.acciones[q].numero_accion));

                }
                if (selectedCondition.acciones[q].tipo_accion == "Activar salida")
                {
                    salidas[selectedCondition.acciones[q].numero_accion].isOn = true;


                }

            }
        }
        shouldContinue = true;

    }

    IEnumerator EsperarNoEntradaX(float x, int valory, Condicion selectedCondition)
    {
        yield return new WaitUntil(() => !entradas[valory].isOn);
        yield return StartCoroutine(comunicacionScript.comprobar_entradas(entradas));
        //yield return new WaitUntil(() => !entradas[valory].isOn|| !entradas[valory].isOn);

        //Condicion selectedCondition = listaCondiciones[i];
        for (float j = 0; j < x; j++)
        {
            for (int q = 0; q < selectedCondition.acciones.Count; q++)
            {
                if (selectedCondition.acciones[q].tipo_accion == "Mover a punto")
                {
                    yield return StartCoroutine(MoverPunto(selectedCondition.acciones[q].numero_accion));

                }
                if (selectedCondition.acciones[q].tipo_accion == "Mover trayectoria")
                {
                    yield return StartCoroutine(MoverTrayectoriaPunto(selectedCondition.acciones[q].numero_accion));
                    yield return StartCoroutine(MoverTrayectoria(selectedCondition.acciones[q].numero_accion));

                }
                if (selectedCondition.acciones[q].tipo_accion == "Activar salida")
                {
                    salidas[selectedCondition.acciones[q].numero_accion].isOn = true;

                }

            }
        }
        shouldContinue = true;

    }



}

