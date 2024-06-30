using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Control_trayectoria_schunk : MonoBehaviour
{
    // Referencia al TMP_Dropdown en tu escena
    public TMP_Dropdown dropdown;
    public TMP_Dropdown dropdown2;
    public Obtener_rotacion_schunk obtenerRotacionScript;
    private int i;
    public float tiempoEntrePuntos = 0.1f;
    public float Velocidad_Porcentaje = 100;
    public float tiempoponderado;
    private float tiempoTranscurrido = 0f;

    private bool PosicionInicial;
    private bool PosicionInicial2;
    private bool HayTrayectoria;

    public TMP_InputField campoEntrada;

    public float velocidad = 100;
    public Toggle Hay_comunicacion;
    public GameObject Comunicacion;
    private Comunicacion_schunk comunicacionesScript;
    private bool inicio = false;

    private bool trajectoryPointCompleted = false;
    private bool trajectoryCompleted = false;

    public Toggle Limite_velocidad;


    // Estructura para almacenar una opción del TMP_Dropdown junto con sus coordenadas


    public struct puntos
    {
        public float value1;
        public float value2;
        public float value3;
        public float value4;
        public float value5;
        public float value6;


        public puntos(float _value1, float _value2, float _value3, float _value4, float _value5, float _value6)
        {
            value1 = _value1;
            value2 = _value2;
            value3 = _value3;
            value4 = _value4;
            value5 = _value5;
            value6 = _value6;
        }
    }

    public struct DropdownOption
    {
        public string name;
        public List<float[]> puntos; // Lista de puntos en la trayectoria
        public List<float> tiemposTransicion; // Lista de tiempos de transición entre puntos

        public DropdownOption(string _name, List<float[]> _puntos, List<float> _tiemposTransicion)
        {
            name = _name;
            puntos = _puntos;
            tiemposTransicion = _tiemposTransicion;
        }
    }

    // Opciones iniciales del TMP_Dropdown
    public List<DropdownOption> options = new List<DropdownOption>();


    public Transform[] articulaciones = new Transform[6]; // Array que contiene todas las articulaciones

    private bool grabandoTrayectoria = false;

    private List<List<Vector3>> trayectorias = new List<List<Vector3>>(); // Lista para almacenar las trayectorias


    // Método para agregar una nueva opción al TMP_Dropdown

    private void Start()
    {
        tiempoponderado = tiempoEntrePuntos;

        comunicacionesScript = Comunicacion.GetComponent<Comunicacion_schunk>();
    }

    public void AddOption()
    {
        // Genera un nombre para la nueva opción
        string newOption = "Trayectoria " + (dropdown.options.Count + 1);

        // Crea una lista vacía para los puntos y los tiempos de transición
        List<float[]> puntos = new List<float[]>();
        List<float> tiemposTransicion = new List<float>();

        // Crea un nuevo DropdownOption con valores predeterminados
        DropdownOption option = new DropdownOption(newOption, puntos, tiemposTransicion);

        // Añade la nueva opción a la lista
        options.Add(option);

        // Actualiza las opciones del TMP_Dropdown
        RefreshDropdownOptions();
    }

    // Método para actualizar las opciones del TMP_Dropdown
    public void RefreshDropdownOptionse()
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

    }

    public void RefreshDropdownOptions()
    {
        // Borra todas las opciones existentes en el TMP_Dropdown
        dropdown.ClearOptions();

        // Agrega las opciones actualizadas al TMP_Dropdown
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (DropdownOption option in options)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(option.name));

            // Si la trayectoria tiene puntos, realizar alguna acción específica
            if (option.puntos.Count > 0)
            {
                // Aquí puedes añadir lógica adicional si es necesario
                Debug.Log("Trayectoria " + option.name + " tiene " + option.puntos.Count + " puntos.");
                // Por ejemplo, podrías añadir puntos adicionales aquí si es necesario
                // option.puntos.Add(nuevoPunto); // Esto es solo un ejemplo
                HayTrayectoria = true;
            }
        }
        dropdown.AddOptions(dropdownOptions);
    }

    public void cambio_punto()
    {
        PosicionInicial = false;
    }

    public void DeleteOption()
    {
        int selectedIndex = dropdown.value;
        if (selectedIndex >= 0 && selectedIndex < options.Count)
        {
            // Eliminar la opción seleccionada
            options.RemoveAt(selectedIndex);

            // Renombrar las opciones restantes
            for (int i = selectedIndex; i < options.Count; i++)
            {
                // Crear una nueva lista para los puntos de la trayectoria y los tiempos de transición
                List<float[]> newPuntos = new List<float[]>(options[i].puntos);
                List<float> newTiemposTransicion = new List<float>(options[i].tiemposTransicion);

                // Asignar la nueva opción con el índice actualizado y los datos de la opción anterior
                options[i] = new DropdownOption("Trayectoria " + (i + 1), newPuntos, newTiemposTransicion);
            }

            // Actualizar las opciones del TMP_Dropdown
            RefreshDropdownOptions();
        }
        else
        {
            Debug.LogWarning("No se ha seleccionado ninguna opción o el índice está fuera de rango.");
        }
    }


    public void InicioGrabacion()
    {
        grabandoTrayectoria = true;

        i = 0;



        //if (grabandoTrayectoria)
        //{
        //    trayectorias.Add(new List<Vector3>()); // Agregar una nueva trayectoria a la lista
        //}
    }

    public void FinGrabacion()
    {
        grabandoTrayectoria = false;
        i = 0;

        Debug.Log("Fin grabacion");

    }

    void Update()
    {


        if (dropdown.value >= 0 && dropdown.value < options.Count)
        {
            //Debug.Log("Nombre de la opción seleccionada: " + options[dropdown.value].name);
            if (grabandoTrayectoria)
            {
                tiempoTranscurrido += Time.deltaTime;
                if (tiempoTranscurrido >= tiempoEntrePuntos || i == 0)
                {
                    DropdownOption trayectoria = options[dropdown.value];


                    float[] nuevoPunto = new float[6];
                    nuevoPunto[0] = obtenerRotacionScript.ObtenerAnguloRotacion1();
                    nuevoPunto[1] = obtenerRotacionScript.ObtenerAnguloRotacion2();
                    nuevoPunto[2] = obtenerRotacionScript.ObtenerAnguloRotacion3();
                    nuevoPunto[3] = obtenerRotacionScript.ObtenerAnguloRotacion4();
                    nuevoPunto[4] = obtenerRotacionScript.ObtenerAnguloRotacion5();
                    nuevoPunto[5] = obtenerRotacionScript.ObtenerAnguloRotacion6();

                    // Agregar el nuevo punto a la trayectoria actual
                    trayectoria.puntos.Add(nuevoPunto);

                    // Reiniciar el tiempo transcurrido
                    tiempoTranscurrido = 0f;


                }
                i = i + 1;
                HayTrayectoria = true;
            }
        }
        else
        {
            // Muestra un mensaje de error si la opción seleccionada no es válida
            //Debug.LogError("¡Opción seleccionada no válida!");
        }

    }

    public void MoverPunto()
    {
        if (Hay_comunicacion.isOn)
        {
            inicio = true;
            Comunicacion_schunk comunicacionIr140 = Comunicacion.GetComponent<Comunicacion_schunk>();
            comunicacionIr140.Punto_Inicial_Trayectoria();
        }
        if (HayTrayectoria == true && grabandoTrayectoria == false)
        {
            MoveToTrayectoria();
            PosicionInicial = true;
            PosicionInicial2 = false;

        }
    }



    public void MoverTrayectoria()
    {
        if (Hay_comunicacion.isOn && inicio == true)
        {
            inicio = false;
            Comunicacion_schunk comunicacionIr140 = Comunicacion.GetComponent<Comunicacion_schunk>();
            comunicacionIr140.Iniciar_Trayectoria();
        }
        if (PosicionInicial == true)
        {

            PosicionInicial2 = true;

        }
    }

    public void MoveToTrayectoria()
    {
        float angulo1;
        Vector3 eje1;
        float angulo2;
        Vector3 eje2;
        float angulo3;
        Vector3 eje3;
        float angulo4;
        Vector3 eje4;
        float angulo5;
        Vector3 eje5;
        float angulo6;
        Vector3 eje6;

        int selectedIndex = dropdown.value;

        DropdownOption trayectoria2 = options[selectedIndex];

        // Calcula las rotaciones deseadas para todos los puntos de la trayectoria
        Quaternion[][] rotacionesDeseadas2 = new Quaternion[trayectoria2.puntos.Count][];
        for (int j = 0; j < trayectoria2.puntos.Count; j++)
        {
            rotacionesDeseadas2[j] = new Quaternion[6]; // 6 rotaciones para cada punto
        }

        if (PosicionInicial == false)
        {
            if (selectedIndex >= 0 && selectedIndex < options.Count && grabandoTrayectoria == false)
            {
                // Obtén la trayectoria seleccionada

                DropdownOption trayectoria = options[selectedIndex];

                // Calcula las rotaciones deseadas para todos los puntos de la trayectoria
                Quaternion[][] rotacionesDeseadas = new Quaternion[trayectoria.puntos.Count][];
                //Quaternion[] rotacionesDeseadas = new Quaternion[6];



                for (int j = 0; j < trayectoria.puntos.Count; j++)
                {

                    Debug.Log("Se inicia la reproduccion" + trayectoria.puntos.Count);
                    rotacionesDeseadas[j] = new Quaternion[6]; // 6 rotaciones para cada punto

                    // Asigna las rotaciones deseadas para cada articulación en el punto actual
                    //Quaternion[] rotacionesDeseadas = new Quaternion[6];

                    angulo1 = trayectoria.puntos[j][0];
                    if (angulo1 < 0)
                    {
                        eje1 = new Vector3(0, 0, 1);
                    }
                    else
                    {
                        eje1 = new Vector3(0, 0, -1);
                    }
                    rotacionesDeseadas[j][0] = Quaternion.AngleAxis(Mathf.Abs(angulo1), eje1);
                    //rotacionesDeseadas[0] = Quaternion.AngleAxis(Mathf.Abs(angulo1), eje1);

                    angulo2 = trayectoria.puntos[j][1];
                    if (angulo2 < 0)
                    {
                        eje2 = new Vector3(0, 1, 0);
                    }
                    else
                    {
                        eje2 = new Vector3(0, -1, 0);
                    }
                    rotacionesDeseadas[j][1] = Quaternion.AngleAxis(Mathf.Abs(angulo2), eje2);
                    //rotacionesDeseadas[1] = Quaternion.AngleAxis(Mathf.Abs(angulo2), eje2);

                    angulo3 = trayectoria.puntos[j][2];
                    if (angulo3 < 0)
                    {
                        eje3 = new Vector3(0, 1, 0);
                    }
                    else
                    {
                        eje3 = new Vector3(0, -1, 0);
                    }
                    rotacionesDeseadas[j][2] = Quaternion.AngleAxis(Mathf.Abs(angulo3), eje3);
                    //rotacionesDeseadas[2] = Quaternion.AngleAxis(Mathf.Abs(angulo3), eje3);

                    angulo4 = trayectoria.puntos[j][3];
                    if (angulo4 < 0)
                    {
                        eje4 = new Vector3(-1, 0, 0);
                    }
                    else
                    {
                        eje4 = new Vector3(1, 0, 0);
                    }
                    rotacionesDeseadas[j][3] = Quaternion.AngleAxis(Mathf.Abs(angulo4), eje4);
                    //rotacionesDeseadas[3] = Quaternion.AngleAxis(Mathf.Abs(angulo4), eje4);

                    angulo5 = trayectoria.puntos[j][4];
                    if (angulo5 < 0)
                    {
                        eje5 = new Vector3(0, 1, 0);
                    }
                    else
                    {
                        eje5 = new Vector3(0, -1, 0);
                    }
                    rotacionesDeseadas[j][4] = Quaternion.AngleAxis(Mathf.Abs(angulo5), eje5);
                    //rotacionesDeseadas[4] = Quaternion.AngleAxis(Mathf.Abs(angulo5), eje5);

                    angulo6 = trayectoria.puntos[j][5];
                    if (angulo6 < 0)
                    {
                        eje6 = new Vector3(-1, 0, 0);
                    }
                    else
                    {
                        eje6 = new Vector3(1, 0, 0);
                    }
                    rotacionesDeseadas[j][5] = Quaternion.AngleAxis(Mathf.Abs(angulo6), eje6);
                    //rotacionesDeseadas[5] = Quaternion.AngleAxis(Mathf.Abs(angulo6), eje6);


                    Debug.Log("Punto1: " + trayectoria.puntos[0][0]);
                    //Debug.Log("Punto: " + rotacionesDeseadas[0]);


                    // Aquí deberías calcular las rotaciones deseadas para cada punto de la trayectoria
                    // Utilizando los valores de trayectoria.puntos[j][0] hasta trayectoria.puntos[j][5]
                    // Puedes usar métodos similares a los que utilizaste para calcular las rotaciones deseadas para un solo punto
                    // Pero adaptados para trabajar con todos los puntos de la trayectoria
                    // Deberías llenar rotacionesDeseadas[j] con las rotaciones calculadas para cada punto de la trayectoria
                    // Aquí incluiré un código de ejemplo que simplemente coloca las articulaciones en la posición inicial
                    // para ilustrar cómo deberías llenar rotacionesDeseadas[j]

                    // Ejemplo de código:
                    // rotacionesDeseadas[j][i] = Quaternion.identity; // Rotación inicial, deberías reemplazar esto con tu lógica real

                }
                StartCoroutine(RotacionGradualInicial(rotacionesDeseadas));

                //StartCoroutine(RotacionGradualTrayectorias(rotacionesDeseadas));

                //RotacionGradualTrayectorias(rotacionesDeseadas);


                // Inicia la rotación gradual para todos los puntos de la trayectoria

                StartCoroutine(WaitForPosicionInicialThenExecute(trayectoria, rotacionesDeseadas));
            }
            else
            {
                // Imprime un mensaje de error si no se ha seleccionado ninguna opción o si el índice está fuera de rango
                Debug.LogError("No se ha seleccionado ninguna opción o el índice está fuera de rango.");
            }

        }
        else
        {
            //StartCoroutine(RotacionGradualTrayectorias(rotacionesDeseadas));
            Debug.Log("Se termina la reproduccion");
        }

    }

    IEnumerator WaitForPosicionInicialThenExecute(DropdownOption trayectoria, Quaternion[][] rotacionesDeseadas)
    {
        while (!PosicionInicial2)
        {
            yield return new WaitForSeconds(0.1f); // Espera un corto período antes de verificar nuevamente
        }

        // Una vez que PosicionInicial sea verdadero, ejecuta la rutina de trayectoria
        StartCoroutine(RotacionGradualTrayectorias(rotacionesDeseadas));

        //PosicionInicial = false;
    }

    IEnumerator RotacionGradualTrayectorias(Quaternion[][] rotacionesDeseadas)
    {
        // Duración de la rotación gradual
        float duracionRotacion;
        float[] distancia = comunicacionesScript.distancia;

        // Calcula las rotaciones iniciales para todas las articulaciones
        Quaternion[][] rotacionesIniciales = new Quaternion[rotacionesDeseadas.Length][];

        if (velocidad > 9 && velocidad < 1001)
        {
            duracionRotacion = 100 * tiempoEntrePuntos / velocidad;
        }
        else
        {
            duracionRotacion = Velocidad_Porcentaje * tiempoEntrePuntos / 100;
        }

        tiempoponderado = duracionRotacion;

        //Debug.Log("El tiempo entre puntos es: "+tiempoponderado);

        //duracionRotacion = Velocidad_Porcentaje * tiempoEntrePuntos / 100;
        //Debug.Log("La velocidad es: "+velocidad);
        // for (int j = 0; j < rotacionesDeseadas.Length; j++)
        //{
        int q = 1;
        rotacionesIniciales[q] = new Quaternion[6]; // 6 rotaciones para cada punto
        for (int w = 0; w < 6; w++)
        {
            rotacionesIniciales[q][w] = articulaciones[w].localRotation;
        }
        // }

        // Itera sobre los puntos de la trayectoria
        for (int j = 1; j < rotacionesDeseadas.Length; j++)
        {
            float tiempoPasado = 0.0f;



            if (Limite_velocidad.isOn && Hay_comunicacion.isOn)
            {
                duracionRotacion = tiempoEntrePuntos;


            }
            else
            {
                if (velocidad > 9 && velocidad < 1001)
                {
                    duracionRotacion = 100 * tiempoEntrePuntos / velocidad;
                }
                else
                {
                    duracionRotacion = Velocidad_Porcentaje * tiempoEntrePuntos / 100;
                }
            }

            while (tiempoPasado < duracionRotacion)
            {
                tiempoPasado += Time.deltaTime;
                float t = Mathf.Clamp01(tiempoPasado / duracionRotacion);

                // Interpola gradualmente entre las rotaciones iniciales y las rotaciones deseadas para todos los puntos de la trayectoria
                for (int w = 0; w < 6; w++)
                {
                    articulaciones[w].localRotation = Quaternion.Lerp(rotacionesIniciales[j][w], rotacionesDeseadas[j][w], t);

                }

                yield return null; // Espera un frame
            }

            if (j < rotacionesDeseadas.Length - 1)
            {
                rotacionesIniciales[j + 1] = new Quaternion[6];
                for (int t = 0; t < 6; t++)
                {
                    rotacionesIniciales[j + 1][t] = rotacionesDeseadas[j][t];
                }

            }

        }

        // Una vez completada la interpolación, podrías realizar alguna acción adicional si es necesario
        Debug.Log("Reproducción de la trayectoria completada.");
        PosicionInicial2 = false;
        PosicionInicial = false;

        trajectoryCompleted = true;
    }

    IEnumerator RotacionGradualInicial(Quaternion[][] rotacionesDeseadas)
    {
        // Duración de la rotación gradual
        float duracionRotacion = velocidad;

        // Calcula las rotaciones iniciales para todas las articulaciones
        Quaternion[][] rotacionesIniciales = new Quaternion[rotacionesDeseadas.Length][];

        // for (int j = 0; j < rotacionesDeseadas.Length; j++)
        //{
        int q = 0;
        rotacionesIniciales[q] = new Quaternion[6]; // 6 rotaciones para cada punto
        for (int w = 0; w < 6; w++)
        {
            rotacionesIniciales[q][w] = articulaciones[w].localRotation;
        }
        // }

        // Itera sobre los puntos de la trayectoria
        for (int j = 0; j < 1; j++)
        {
            float tiempoPasado = 0.0f; // Tiempo transcurrido desde el inicio de la rotación gradual
            float veloc = comunicacionesScript.velocidad;


            if (Limite_velocidad.isOn || Hay_comunicacion.isOn)
            {
                duracionRotacion = 3;


            }
            else
            {
                if (velocidad > 9 && velocidad < 1001)
                {
                    duracionRotacion = 100 * 1 / velocidad;
                }
                else
                {
                    duracionRotacion = Velocidad_Porcentaje * 1 / 100;
                }

            }


            while (tiempoPasado < duracionRotacion)
            {
                tiempoPasado += Time.deltaTime;
                float t = Mathf.Clamp01(tiempoPasado / duracionRotacion);

                // Interpola gradualmente entre las rotaciones iniciales y las rotaciones deseadas para todos los puntos de la trayectoria
                for (int w = 0; w < 6; w++)
                {
                    articulaciones[w].localRotation = Quaternion.Lerp(rotacionesIniciales[j][w], rotacionesDeseadas[j][w], t);

                }

                yield return null; // Espera un frame
            }


        }

        // Una vez completada la interpolación, podrías realizar alguna acción adicional si es necesario
        Debug.Log("Posicionado en punto inicial");
        trajectoryPointCompleted = true;
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

            velocidad = 100;
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
            List<float[]> puntosCopia = new List<float[]>(option.puntos);
            dropdownOptions.Add(new TMP_Dropdown.OptionData(option.name));
        }

        // Agregar opciones al dropdown2
        dropdown2.AddOptions(dropdownOptions);
    }


    public void MoverPuntoIndex(int index)
    {
        trajectoryPointCompleted = false;
        if (Hay_comunicacion.isOn)
        {
            //inicio = true;
            //Comunicacion_irb140 comunicacionIr140 = Comunicacion.GetComponent<Comunicacion_irb140>();
            //comunicacionIr140.Punto_Inicial_Trayectoria();
        }
        if (HayTrayectoria == true && grabandoTrayectoria == false)
        {
            MoveToTrayectoriaIndex(index);
            PosicionInicial = true;
            PosicionInicial2 = false;

        }
    }

    public void MoverTrayectoriaIndex(int index)
    {
        trajectoryCompleted = false;
        if (Hay_comunicacion.isOn && inicio == true)
        {
            //inicio = false;
            //Comunicacion_irb140 comunicacionIr140 = Comunicacion.GetComponent<Comunicacion_irb140>();
            //comunicacionIr140.Iniciar_Trayectoria();
        }
        if (PosicionInicial == true)
        {

            PosicionInicial2 = true;

        }
    }

    public void MoveToTrayectoriaIndex(int index)
    {
        float angulo1;
        Vector3 eje1;
        float angulo2;
        Vector3 eje2;
        float angulo3;
        Vector3 eje3;
        float angulo4;
        Vector3 eje4;
        float angulo5;
        Vector3 eje5;
        float angulo6;
        Vector3 eje6;

        int selectedIndex = index;

        DropdownOption trayectoria2 = options[selectedIndex];

        // Calcula las rotaciones deseadas para todos los puntos de la trayectoria
        Quaternion[][] rotacionesDeseadas2 = new Quaternion[trayectoria2.puntos.Count][];
        for (int j = 0; j < trayectoria2.puntos.Count; j++)
        {
            rotacionesDeseadas2[j] = new Quaternion[6]; // 6 rotaciones para cada punto
        }

        if (PosicionInicial == false)
        {
            if (selectedIndex >= 0 && selectedIndex < options.Count && grabandoTrayectoria == false)
            {
                // Obtén la trayectoria seleccionada

                DropdownOption trayectoria = options[selectedIndex];

                // Calcula las rotaciones deseadas para todos los puntos de la trayectoria
                Quaternion[][] rotacionesDeseadas = new Quaternion[trayectoria.puntos.Count][];
                //Quaternion[] rotacionesDeseadas = new Quaternion[6];



                for (int j = 0; j < trayectoria.puntos.Count; j++)
                {

                    Debug.Log("Se inicia la reproduccion");
                    rotacionesDeseadas[j] = new Quaternion[6]; // 6 rotaciones para cada punto

                    // Asigna las rotaciones deseadas para cada articulación en el punto actual
                    //Quaternion[] rotacionesDeseadas = new Quaternion[6];

                    angulo1 = trayectoria.puntos[j][0];
                    if (angulo1 < 0)
                    {
                        eje1 = new Vector3(0, 0, 1);
                    }
                    else
                    {
                        eje1 = new Vector3(0, 0, -1);
                    }
                    rotacionesDeseadas[j][0] = Quaternion.AngleAxis(Mathf.Abs(angulo1), eje1);
                    //rotacionesDeseadas[0] = Quaternion.AngleAxis(Mathf.Abs(angulo1), eje1);

                    angulo2 = trayectoria.puntos[j][1];
                    if (angulo2 < 0)
                    {
                        eje2 = new Vector3(0, 1, 0);
                    }
                    else
                    {
                        eje2 = new Vector3(0, -1, 0);
                    }
                    rotacionesDeseadas[j][1] = Quaternion.AngleAxis(Mathf.Abs(angulo2), eje2);
                    //rotacionesDeseadas[1] = Quaternion.AngleAxis(Mathf.Abs(angulo2), eje2);

                    angulo3 = trayectoria.puntos[j][2];
                    if (angulo3 < 0)
                    {
                        eje3 = new Vector3(0, 1, 0);
                    }
                    else
                    {
                        eje3 = new Vector3(0, -1, 0);
                    }
                    rotacionesDeseadas[j][2] = Quaternion.AngleAxis(Mathf.Abs(angulo3), eje3);
                    //rotacionesDeseadas[2] = Quaternion.AngleAxis(Mathf.Abs(angulo3), eje3);

                    angulo4 = trayectoria.puntos[j][3];
                    if (angulo4 < 0)
                    {
                        eje4 = new Vector3(-1, 0, 0);
                    }
                    else
                    {
                        eje4 = new Vector3(1, 0, 0);
                    }
                    rotacionesDeseadas[j][3] = Quaternion.AngleAxis(Mathf.Abs(angulo4), eje4);
                    //rotacionesDeseadas[3] = Quaternion.AngleAxis(Mathf.Abs(angulo4), eje4);

                    angulo5 = trayectoria.puntos[j][4];
                    if (angulo5 < 0)
                    {
                        eje5 = new Vector3(0, 1, 0);
                    }
                    else
                    {
                        eje5 = new Vector3(0, -1, 0);
                    }
                    rotacionesDeseadas[j][4] = Quaternion.AngleAxis(Mathf.Abs(angulo5), eje5);
                    //rotacionesDeseadas[4] = Quaternion.AngleAxis(Mathf.Abs(angulo5), eje5);

                    angulo6 = trayectoria.puntos[j][5];
                    if (angulo6 < 0)
                    {
                        eje6 = new Vector3(-1, 0, 0);
                    }
                    else
                    {
                        eje6 = new Vector3(1, 0, 0);
                    }
                    rotacionesDeseadas[j][5] = Quaternion.AngleAxis(Mathf.Abs(angulo6), eje6);
                    //rotacionesDeseadas[5] = Quaternion.AngleAxis(Mathf.Abs(angulo6), eje6);


                    Debug.Log("Punto1: " + trayectoria.puntos[0][0]);
                    //Debug.Log("Punto: " + rotacionesDeseadas[0]);


                    // Aquí deberías calcular las rotaciones deseadas para cada punto de la trayectoria
                    // Utilizando los valores de trayectoria.puntos[j][0] hasta trayectoria.puntos[j][5]
                    // Puedes usar métodos similares a los que utilizaste para calcular las rotaciones deseadas para un solo punto
                    // Pero adaptados para trabajar con todos los puntos de la trayectoria
                    // Deberías llenar rotacionesDeseadas[j] con las rotaciones calculadas para cada punto de la trayectoria
                    // Aquí incluiré un código de ejemplo que simplemente coloca las articulaciones en la posición inicial
                    // para ilustrar cómo deberías llenar rotacionesDeseadas[j]

                    // Ejemplo de código:
                    // rotacionesDeseadas[j][i] = Quaternion.identity; // Rotación inicial, deberías reemplazar esto con tu lógica real

                }
                StartCoroutine(RotacionGradualInicial_p(rotacionesDeseadas, index));

                //StartCoroutine(RotacionGradualTrayectorias(rotacionesDeseadas));

                //RotacionGradualTrayectorias(rotacionesDeseadas);


                // Inicia la rotación gradual para todos los puntos de la trayectoria

                StartCoroutine(WaitForPosicionInicialThenExecute_p(trayectoria, rotacionesDeseadas, index));
            }
            else
            {
                // Imprime un mensaje de error si no se ha seleccionado ninguna opción o si el índice está fuera de rango
                Debug.LogError("No se ha seleccionado ninguna opción o el índice está fuera de rango.");
            }

        }
        else
        {
            //StartCoroutine(RotacionGradualTrayectorias(rotacionesDeseadas));
            Debug.Log("Se termina la reproduccion");
        }

    }


    IEnumerator RotacionGradualInicial_p(Quaternion[][] rotacionesDeseadas, int index)
    {
        // Duración de la rotación gradual
        float duracionRotacion = velocidad;

        // Calcula las rotaciones iniciales para todas las articulaciones
        Quaternion[][] rotacionesIniciales = new Quaternion[rotacionesDeseadas.Length][];

        // for (int j = 0; j < rotacionesDeseadas.Length; j++)
        //{
        int q = 0;
        rotacionesIniciales[q] = new Quaternion[6]; // 6 rotaciones para cada punto
        for (int w = 0; w < 6; w++)
        {
            rotacionesIniciales[q][w] = articulaciones[w].localRotation;
        }
        // }

        // Itera sobre los puntos de la trayectoria
        for (int j = 0; j < 1; j++)
        {
            float tiempoPasado = 0.0f; // Tiempo transcurrido desde el inicio de la rotación gradual
            float veloc = comunicacionesScript.velocidad;


            if (Limite_velocidad.isOn && Hay_comunicacion.isOn)
            {

                float distancia = comunicacionesScript.distancias[index, j];
                // recibo la distancia del scrip comunicacion
                duracionRotacion = distancia / 190;
                Debug.Log("index:" + index);

                Debug.Log("Distancia:" + distancia);
                Debug.Log("Duracion Rotacion:" + duracionRotacion);


            }
            else
            {
                if (velocidad > 9 && velocidad < 1001)
                {
                    duracionRotacion = 100 * 1 / velocidad;
                }
                else
                {
                    duracionRotacion = Velocidad_Porcentaje * 1 / 100;
                }

            }

            duracionRotacion = 3.0f;

            while (tiempoPasado < duracionRotacion)
            {
                tiempoPasado += Time.deltaTime;
                float t = Mathf.Clamp01(tiempoPasado / duracionRotacion);

                // Interpola gradualmente entre las rotaciones iniciales y las rotaciones deseadas para todos los puntos de la trayectoria
                for (int w = 0; w < 6; w++)
                {
                    articulaciones[w].localRotation = Quaternion.Lerp(rotacionesIniciales[j][w], rotacionesDeseadas[j][w], t);

                }

                yield return null; // Espera un frame
            }


        }

        // Una vez completada la interpolación, podrías realizar alguna acción adicional si es necesario
        Debug.Log("Posicionado en punto inicial");
        trajectoryPointCompleted = true;
    }

    IEnumerator WaitForPosicionInicialThenExecute_p(DropdownOption trayectoria, Quaternion[][] rotacionesDeseadas, int index)
    {
        while (!PosicionInicial2)
        {
            yield return new WaitForSeconds(0.1f); // Espera un corto período antes de verificar nuevamente
        }

        // Una vez que PosicionInicial sea verdadero, ejecuta la rutina de trayectoria
        StartCoroutine(RotacionGradualTrayectorias_p(rotacionesDeseadas, index));

        //PosicionInicial = false;
    }

    IEnumerator RotacionGradualTrayectorias_p(Quaternion[][] rotacionesDeseadas, int index)
    {
        // Duración de la rotación gradual
        float duracionRotacion;
        float[] distancia = comunicacionesScript.distancia;

        // Calcula las rotaciones iniciales para todas las articulaciones
        Quaternion[][] rotacionesIniciales = new Quaternion[rotacionesDeseadas.Length][];

        if (velocidad > 9 && velocidad < 1001)
        {
            duracionRotacion = 100 * tiempoEntrePuntos / velocidad;
        }
        else
        {
            duracionRotacion = Velocidad_Porcentaje * tiempoEntrePuntos / 100;
        }

        tiempoponderado = duracionRotacion;

        //Debug.Log("El tiempo entre puntos es: "+tiempoponderado);

        //duracionRotacion = Velocidad_Porcentaje * tiempoEntrePuntos / 100;
        //Debug.Log("La velocidad es: "+velocidad);
        // for (int j = 0; j < rotacionesDeseadas.Length; j++)
        //{
        int q = 1;
        rotacionesIniciales[q] = new Quaternion[6]; // 6 rotaciones para cada punto
        for (int w = 0; w < 6; w++)
        {
            rotacionesIniciales[q][w] = articulaciones[w].localRotation;
        }
        // }

        // Itera sobre los puntos de la trayectoria
        for (int j = 1; j < rotacionesDeseadas.Length; j++)
        {
            float tiempoPasado = 0.0f;



            if (Limite_velocidad.isOn && Hay_comunicacion.isOn)
            {
                distancia[j] = comunicacionesScript.distancias[index, j];
                // recibo la distancia del scrip comunicacion
                duracionRotacion = distancia[j] / 190;


            }
            else
            {
                if (velocidad > 9 && velocidad < 1001)
                {
                    duracionRotacion = 100 * tiempoEntrePuntos / velocidad;
                }
                else
                {
                    duracionRotacion = Velocidad_Porcentaje * tiempoEntrePuntos / 100;
                }
            }

            while (tiempoPasado < duracionRotacion)
            {
                tiempoPasado += Time.deltaTime;
                float t = Mathf.Clamp01(tiempoPasado / duracionRotacion);

                // Interpola gradualmente entre las rotaciones iniciales y las rotaciones deseadas para todos los puntos de la trayectoria
                for (int w = 0; w < 6; w++)
                {
                    articulaciones[w].localRotation = Quaternion.Lerp(rotacionesIniciales[j][w], rotacionesDeseadas[j][w], t);

                }

                yield return null; // Espera un frame
            }

            if (j < rotacionesDeseadas.Length - 1)
            {
                rotacionesIniciales[j + 1] = new Quaternion[6];
                for (int t = 0; t < 6; t++)
                {
                    rotacionesIniciales[j + 1][t] = rotacionesDeseadas[j][t];
                }

            }

        }

        // Una vez completada la interpolación, podrías realizar alguna acción adicional si es necesario
        Debug.Log("Reproducción de la trayectoria completada.");
        PosicionInicial2 = false;
        PosicionInicial = false;

        trajectoryCompleted = true;
    }



    public bool HasCompletedTrajectoryPoint(int index)
    {
        // Devuelve true si la trayectoria ha sido completada
        return trajectoryPointCompleted;
    }
    public bool HasCompletedTrajectory(int index)
    {
        // Devuelve true si la trayectoria ha sido completada
        return trajectoryCompleted;
    }

}

