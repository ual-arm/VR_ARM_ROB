using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.Http;
using System.Threading;
//using UnityEditor.VersionControl;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class Comunicacion_irb1090 : MonoBehaviour
{
    // Start is called before the first frame update
    //public string robotStudioIP = "192.168.144.150";
    public string robotStudioIP = "127.0.0.1";
    public int robotStudioTcpPort = 11000;
    public TcpClient tcpClient;
    NetworkStream tcpStream;
    public float tiempo_envio = 0.3f;
    public TMP_Dropdown dropdown;
    public TMP_Dropdown dropdown_trayectorias;
    public TMP_Dropdown dropdown_puntos;
    public float velocidad;

    public bool ActiveC;
    private bool loopsend;

    public Transform Articulacion1;
    public Transform Articulacion2;
    public Transform Articulacion3;
    public Transform Articulacion4;
    public Transform Articulacion5;
    public Transform Articulacion6;


    private TcpListener tcpListener;

    private int modo = 6;

    private TMP_DropdownManager_irb1090 dropdownManager;
    public Control_trayectoria_irb1090 scriptTrayectoria;
    private TMP_Dropdown dropdownManager2;

    private bool guardando = false;

    private bool cargando = false;

    private bool envioCompleto = false;
    private bool envioCompleto2 = false;

    //public string filePoint = "Assets/YourFolder/YourFile.txt";
    //public string fileTrayectoria = "Assets/YourFolder/YourFile.txt";
    private string nombreArchivopuntos = "Puntos_irb1090.txt";
    private string nombreArchivotrayectoria = "Trayectorias_irb1090.txt";

    string directorioProyecto;
    string rutaCompletapunto;
    string rutaCompletatrayectoria;

    public GameObject transfiriendo;
    public GameObject transfiriendo2;

    public GameObject programacion;
    private Control_programacion_irb1090 scriptprogramacion;

    private TcpClient connectedTcpClient;

    public List<Control_programacion_irb1090.Condicion> listaCondiciones = new List<Control_programacion_irb1090.Condicion>();

    public Toggle Limite_velocidad;
    public GameObject Limite_velocidad_;

    public float[] distancia = new float[600];
    public float[,] distancias = new float[20, 600];
    public float[] velocidades = new float[600];
    public float[,] velocidades_p = new float[20, 600];
    public float[] tiempo_ = new float[100];

    void Start()
    {
        directorioProyecto = Directory.GetParent(Application.dataPath).FullName;

        rutaCompletapunto = Path.Combine(directorioProyecto, nombreArchivopuntos);
        if (!File.Exists(rutaCompletapunto))
        {
            Debug.LogError("El archivo no existe: ");
            return;
        }

        rutaCompletatrayectoria = Path.Combine(directorioProyecto, nombreArchivotrayectoria);
        if (!File.Exists(rutaCompletatrayectoria))
        {
            Debug.LogError("El archivo no existe: ");
            return;
        }


        ReadPoint();
        LoadTrajectories();
        scriptprogramacion = programacion.GetComponent<Control_programacion_irb1090>();
        scriptprogramacion.LoadPrograma();

    }

    public void DropdownValueChanged(TMP_Dropdown change)
    {
        modo = change.value;

        Debug.Log("modo: " + modo);

        if (ActiveC && modo != 0)
        {
            //ActiveC = true;
            //loopsend = true;

            FinalizarConexion();
        }
        if (ActiveC && modo == 1)
        {
            loopsend = true;
        }

    }


    public void IniciarConexion()
    {

        Debug.Log("Iniciando conexion.");
        // Creamos una conexion con el servidor indicado en la diteccion ip y puerto
        // He probado a definir las variables arriba.
        //tcpClient = new TcpClient(robotStudioIP, robotStudioTcpPort);
        //tcpStream = tcpClient.GetStream();

        try
        {
            if (ActiveC == false)
            {

                tcpClient = new TcpClient(robotStudioIP, robotStudioTcpPort);
                tcpStream = tcpClient.GetStream();

                // Creamos un mensaje que convertimos a bytes mediante el codigo UTF8
                // y guardamos en la variable data, la cual enviamos por el stream.
                string message = "Hello, server";
                byte[] data = Encoding.UTF8.GetBytes(message);
                tcpStream.Write(data, 0, data.Length);

                Debug.Log("Mensaje enviado");

                // La informacion que recibo de robotstudio por el canal de stream lo almaceno
                // en buffer y lo convierto a una cadena de caracterres con UTF8, almacenada en response
                byte[] buffer = new byte[1024];
                int bytesRead = tcpStream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Debug.Log("Mensaje recibido");

                if (response == "Hello")
                {
                    // Si la respuesta del servidor robotStudio es correcta inicion una conexion UDP
                    // En un hilo secundario distinto al Tcp, por donde enviare las coordenadas del robot.
                    Debug.Log("Conexion correcta");
                    ActiveC = true;
                    //loopsend = true;

                    if (guardando == false && cargando == false)
                    {
                        DropdownValueChanged(dropdown);
                    }
                    if (modo == 0)
                    {
                        loopsend = true;
                        Limite_velocidad_.SetActive(false);
                        StartCoroutine(EjecutarCadaSegundo());
                        //StartCoroutine(EjecutarCadaSegundo());
                    }
                    if (modo == 1)
                    {
                        //Envio_punto();
                    }
                    if (modo == 2)
                    {

                        Envio_trayectoria();
                        //StartCoroutine(EjecutarCadaSegundo());
                    }
                    if (modo == 3)
                    {
                        Envio_datos_programa();
                    }

                }
                else
                {
                    Debug.LogError("Error de conexion");

                    ActiveC = false;
                }

            }
            else
            {

                if (guardando == false && modo == 0)
                {
                    loopsend = true;
                    Limite_velocidad_.SetActive(false);
                    StartCoroutine(EjecutarCadaSegundo());
                }
                if (modo == 1)
                {
                    //Envio_punto();
                }
                if (modo == 2)
                {
                    Envio_trayectoria();
                }
                if (modo == 3)
                {
                    Envio_datos_programa();
                }

            }

        }
        catch (Exception e)
        {
            Debug.LogError("Error de conexion: " + e.Message);
            //ActiveC = false;
            //tcpClient.Close();
            //tcpClient = null;
            //tcpStream.Close();

            FinalizarConexion();
        }
    }

    public void IniciarConexion_servidor2()
    {
        //StartCoroutine(IniciarConexion_servidor_2());
    }
    public void IniciarConexion_servidor()
    {



        if (ActiveC == false)
        {

            tcpListener = new TcpListener(IPAddress.Any, robotStudioTcpPort);
            tcpListener.Start();

            Debug.Log("Escuchando conexiones entrantes...");


            //while (tcpListener.Pending())
            //{
            Debug.Log("Cliente conectado.");
            connectedTcpClient = tcpListener.AcceptTcpClient();
            tcpStream = connectedTcpClient.GetStream();
            Debug.Log("Cliente conectado.");
            //StartCoroutine(HandleClientComm(connectedTcpClient));
            // }
            byte[] buffer2 = new byte[1024];
            int bytesRead2 = tcpStream.Read(buffer2, 0, buffer2.Length);
            string response2 = Encoding.UTF8.GetString(buffer2, 0, bytesRead2);
            if (response2 == "Prueba")
            {
                string message3 = "ok";
                byte[] data3 = Encoding.UTF8.GetBytes(message3);
                tcpStream.Write(data3, 0, data3.Length);
            }
            // Creamos un mensaje que convertimos a bytes mediante el codigo UTF8
            // y guardamos en la variable data, la cual enviamos por el stream.
            string message = "Hello, server";
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpStream.Write(data, 0, data.Length);

            Debug.Log("Mensaje enviado");

            // La informacion que recibo de robotstudio por el canal de stream lo almaceno
            // en buffer y lo convierto a una cadena de caracterres con UTF8, almacenada en response
            byte[] buffer = new byte[1024];
            int bytesRead = tcpStream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Debug.Log("Mensaje recibido");

            if (response == "Hello")
            {
                // Si la respuesta del servidor robotStudio es correcta inicion una conexion UDP
                // En un hilo secundario distinto al Tcp, por donde enviare las coordenadas del robot.
                Debug.Log("Conexion correcta");
                ActiveC = true;
                //loopsend = true;

                if (guardando == false && cargando == false)
                {
                    DropdownValueChanged(dropdown);
                }
                if (modo == 0)
                {
                    loopsend = true;
                    Limite_velocidad_.SetActive(false);
                    StartCoroutine(EjecutarCadaSegundo());
                    //StartCoroutine(EjecutarCadaSegundo());
                }
                if (modo == 2)
                {

                    Envio_trayectoria();
                    //StartCoroutine(EjecutarCadaSegundo());
                }
                if (modo == 3)
                {
                    Envio_datos_programa();
                }

            }
            else
            {
                Debug.LogError("Error de conexion");

                ActiveC = false;
            }

        }
        else
        {

            if (guardando == false && modo == 0)
            {
                loopsend = true;
                Limite_velocidad_.SetActive(false);
                StartCoroutine(EjecutarCadaSegundo());
            }
            if (modo == 1)
            {
                //Envio_punto();
            }
            if (modo == 2)
            {
                Envio_trayectoria();
            }
            if (modo == 3)
            {
                Envio_datos_programa();
            }

        }

        //yield return null;

    }

    IEnumerator HandleClientComm(TcpClient tcpClient)
    {
        NetworkStream clientStream = tcpClient.GetStream();
        byte[] message = new byte[4096];
        int bytesRead;

        while (true)
        {
            if (clientStream.DataAvailable)
            {
                bytesRead = clientStream.Read(message, 0, message.Length);
                if (bytesRead == 0)
                {
                    break; // Cliente se ha desconectado
                }

                string clientMessage = Encoding.UTF8.GetString(message, 0, bytesRead);
                Debug.Log("Mensaje recibido: " + clientMessage);

                byte[] responseMessage = Encoding.UTF8.GetBytes("Hello from server");
                clientStream.Write(responseMessage, 0, responseMessage.Length);
            }
            yield return null;
        }

        tcpClient.Close();
    }


    public void FinalizarConexiones()
    {
        Debug.Log("Finalizando conexion.");

        // Usamos el canal existente para avisar al servidor de que vamos a cerrar conexion.



        if (tcpClient != null && tcpStream != null)
        {

            string message = "Close";
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpStream.Write(data, 0, data.Length);

            ActiveC = false;


            tcpClient.Close();
            tcpClient = null;
            tcpStream.Close();
        }

    }

    public void FinalizarConexion()
    {
        Debug.Log("Finalizando conexion.");



        // Usamos el canal existente para avisar al servidor de que vamos a cerrar conexion.

        loopsend = false;
        //guardando = false;

    }



    IEnumerator EjecutarCadaSegundo()
    {
        string message = "m";
        byte[] data = Encoding.UTF8.GetBytes(message);
        tcpStream.Write(data, 0, data.Length);
        //while (loopsend == true&& modo==0)
        byte[] buffercontrol1 = new byte[1024];
        int bytesRecived1 = tcpStream.Read(buffercontrol1, 0, buffercontrol1.Length);
        while (modo == 0 && loopsend == true)
        {

            float J1 = Articulacion1.localEulerAngles.z;
            float J2 = Articulacion2.localEulerAngles.y;
            float J3 = Articulacion3.localEulerAngles.y;
            float J4 = Articulacion4.localEulerAngles.y;
            float J5 = Articulacion5.localEulerAngles.y;
            float J6 = Articulacion5.localEulerAngles.y;

            // Crear el mensaje UDP con los ?ngulos y enviarlo al servidor
            string CoordenateMessage = string.Format("{0};{1};{2};{3};{4};{5}", J1, J2, J3, J4, J5, J6);
            byte[] CoordenateData = Encoding.UTF8.GetBytes(CoordenateMessage);

            tcpStream.Write(CoordenateData, 0, CoordenateData.Length);

            byte[] buffercontrol2 = new byte[1024];

            yield return new WaitForSeconds(tiempo_envio);

            // Realiza la acci?n que deseas ejecutar cada segundo
        }

        if (tcpClient != null && tcpStream != null && guardando != true)
        {

            string message1 = "fm";
            byte[] data1 = Encoding.UTF8.GetBytes(message1);
            tcpStream.Write(data1, 0, data1.Length);

            byte[] buffercontrol = new byte[1024];

        }
    }

    // Destruye la conexion cuando se cierra la aplicacion
    void OnDestroy()
    {

        if (tcpClient != null && tcpStream != null)
        {

            string message = "Close";
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpStream.Write(data, 0, data.Length);

            ActiveC = false;


            tcpClient.Close();
            tcpClient = null;
            tcpStream.Close();
        }
    }




    public void Envio_punto()
    {
        envioCompleto = false;
        if (ActiveC == true)
        {
            guardando = true;
            string message = "mp";
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpStream.Write(data, 0, data.Length);
            Debug.Log("Inicio guardado");
            byte[] buffercontrol = new byte[1024];
            int bytesRecived = tcpStream.Read(buffercontrol, 0, buffercontrol.Length);

            //Envio_g();

            //StartCoroutine(Envio_g());
            StartCoroutine(SendSelectedPoint());
            //StartCoroutine(EsperarEnvioCompleto());

        }
        else
        {
            Debug.Log("El robot no esta conectado");
        }


    }


    public IEnumerator SendSelectedPoint()
    {
        // Obtener el índice del punto seleccionado en el dropdown
        int selectedIndex = dropdown_puntos.value;

        float tiempo;

        // Verificar que el índice sea válido
        if (selectedIndex < 0 || selectedIndex >= dropdownManager.options.Count)
        {
            Debug.LogError("Índice de selección fuera de rango.");
            yield break;
        }

        // Obtener la opción seleccionada
        TMP_DropdownManager_irb1090.DropdownOption selectedOption = dropdownManager.options[selectedIndex];

        // Obtener los valores de los ángulos del punto seleccionado
        float J1 = selectedOption.value1;
        float J2 = selectedOption.value2;
        float J3 = selectedOption.value3;
        float J4 = selectedOption.value4;
        float J5 = selectedOption.value5;
        float J6 = selectedOption.value6;

        // Crear el mensaje UDP con los ángulos y enviarlo al servidor
        string CoordenateMessage = string.Format("{0};{1};{2};{3};{4};{5}", J1, J2, J3, J4, J5, J6);
        byte[] CoordenateData = Encoding.UTF8.GetBytes(CoordenateMessage);
        tcpStream.Write(CoordenateData, 0, CoordenateData.Length);
        byte[] buffer = new byte[1024];
        int bytesRead = tcpStream.Read(buffer, 0, buffer.Length);
        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        response = response.Replace('.', ',');
        float.TryParse(response, out distancia[0]);
        velocidad = distancia[0] / (dropdownManager.velocidad);
        Debug.Log("Distancia: " + distancia[0]);
        Debug.Log("Velocidad: " + velocidad);
        Debug.Log("Tiempo: " + dropdownManager.velocidad);


        if (Limite_velocidad.isOn)
        {
            if (velocidad > 190)
            {
                velocidad = 190;
            }
        }


        string message = velocidad.ToString();
        message = message.Replace(',', '.');
        byte[] data = Encoding.UTF8.GetBytes(message);
        tcpStream.Write(data, 0, data.Length);

        byte[] buffercontrol2 = new byte[1024];
        int bytesRecived2 = tcpStream.Read(buffercontrol2, 0, buffercontrol2.Length);

        // Esperar un tiempo antes de continuar, si es necesario
        //yield return new WaitForSeconds(tiempo_envio);
        envioCompleto = true;
    }

    private IEnumerator EsperarEnvioCompleto()
    {
        // Espera hasta que el envío esté completo
        yield return new WaitUntil(() => envioCompleto);

        // Envío completo, finaliza la conexión u realiza otras acciones necesarias
        string message1 = "fgp";
        byte[] data1 = Encoding.UTF8.GetBytes(message1);
        tcpStream.Write(data1, 0, data1.Length);
        Debug.Log("Finalizar guardado");
        guardando = false;

        byte[] buffercontrol1 = new byte[1024];
        int bytesRecived1 = tcpStream.Read(buffercontrol1, 0, buffercontrol1.Length);

        FinalizarConexion();
    }


    public void Envio_trayectoria()
    {
        envioCompleto = false;
        if (ActiveC == true && modo == 2)
        {
            transfiriendo.SetActive(true);
            guardando = true;
            string message = "mt";
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpStream.Write(data, 0, data.Length);
            Debug.Log("Inicio guardado trayectoria");
            byte[] buffercontrol = new byte[1024];
            int bytesRecived = tcpStream.Read(buffercontrol, 0, buffercontrol.Length);

            //Envio_g();

            //StartCoroutine(Envio_g());
            StartCoroutine(SendSelectedTrajectory());
            StartCoroutine(EsperarEnvioCompletoT());

        }
        else
        {
            Debug.Log("El robot no esta conectado");
        }


    }

    public IEnumerator SendSelectedTrajectory()
    {

        GameObject trayectoriasObject = GameObject.Find("Trayectorias_irb1090");
        Control_trayectoria_irb1090 controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_irb1090>();

        if (trayectoriasObject != null)
        {
            controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_irb1090>();
            if (controlTrayectoriaScript == null)
            {
                Debug.LogError("No se encontró el script Control_trayectoria en el GameObject 'Trayectorias'");
            }

        }
        float tiempo = controlTrayectoriaScript.tiempoponderado;
        float velocidadMaxima;
        float duracionRotacion;


        int selectedIndex = dropdown_trayectorias.value;
        Debug.Log("El indice es: " + selectedIndex);
        // Verificar que el índice sea válido
        if (selectedIndex < 0 || selectedIndex >= controlTrayectoriaScript.options.Count)
        {
            Debug.LogError("Índice de selección fuera de rango.");
            yield break;
        }

        // Obtener la trayectoria seleccionada
        Control_trayectoria_irb1090.DropdownOption selectedTrajectory = controlTrayectoriaScript.options[selectedIndex];

        // Obtener la lista de puntos de la trayectoria seleccionada
        List<float[]> puntos = selectedTrajectory.puntos;

        // Crear el mensaje inicial con el número de puntos
        string numberOfPointsMessage = puntos.Count.ToString();
        byte[] numberOfPointsData = Encoding.UTF8.GetBytes(numberOfPointsMessage);

        // Enviar el mensaje inicial con el número de puntos
        tcpStream.Write(numberOfPointsData, 0, numberOfPointsData.Length);
        Debug.Log("Número de puntos enviado: " + numberOfPointsMessage);

        byte[] buffercontrol = new byte[1024];
        int bytesRecived = tcpStream.Read(buffercontrol, 0, buffercontrol.Length);

        string message = tiempo.ToString();
        byte[] data = Encoding.UTF8.GetBytes(message);
        tcpStream.Write(data, 0, data.Length);

        byte[] buffercontrol2 = new byte[1024];
        int bytesRecived2 = tcpStream.Read(buffercontrol2, 0, buffercontrol2.Length);

        // Esperar un tiempo antes de continuar, si es necesario
        yield return new WaitForSeconds(tiempo_envio);

        // Iterar sobre los puntos de la trayectoria y enviar cada punto
        int i = 0;
        foreach (float[] punto in puntos)
        {

            // Crear el mensaje UDP con las coordenadas del punto y enviarlo al servidor
            string CoordenateMessage = string.Format("{0};{1};{2};{3};{4};{5}",
                punto[0], punto[1], punto[2], punto[3], punto[4], punto[5]);
            byte[] CoordenateData = Encoding.UTF8.GetBytes(CoordenateMessage);

            tcpStream.Write(CoordenateData, 0, CoordenateData.Length);

            Debug.Log("Punto enviado: " + CoordenateMessage);

            byte[] buffer = new byte[1024];
            int bytesRead = tcpStream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            response = response.Replace(".", ",");
            float.TryParse(response, out distancia[i]);
            if (distancia[i] > 0)
            {
                velocidad = distancia[i] / (19 / (scriptTrayectoria.velocidad));
            }
            else
            {
                velocidad = 190;
            }


            if (Limite_velocidad.isOn)
            {
                if (velocidad > 190)
                {
                    velocidades[i] = 190;
                }
            }
            //velocidades[i] = velocidad;
            Debug.Log("Distancia: " + distancia[i]);
            // Debug.Log("Velocidad: " + velociades[i]);
            string message3 = velocidad.ToString();
            message3 = message3.Replace(',', '.');
            message3 = message3.Replace(',', '.');
            byte[] data3 = Encoding.UTF8.GetBytes(message3);
            tcpStream.Write(data3, 0, data3.Length);

            byte[] buffercontrol3 = new byte[1024];
            int bytesRecived3 = tcpStream.Read(buffercontrol3, 0, buffercontrol3.Length);

            string message4 = tiempo.ToString();
            byte[] data4 = Encoding.UTF8.GetBytes(message3);
            tcpStream.Write(data3, 0, data3.Length);

            byte[] buffercontrol4 = new byte[1024];
            int bytesRecived4 = tcpStream.Read(buffercontrol4, 0, buffercontrol4.Length);

            // Esperar un tiempo antes de continuar, si es necesarioº
            yield return new WaitForSeconds(tiempo_envio / 4);
            i = i + 1;
        }

        envioCompleto = true;
    }


    private IEnumerator EsperarEnvioCompletoT()
    {
        // Espera hasta que el envío esté completo
        yield return new WaitUntil(() => envioCompleto);

        // Envío completo, finaliza la conexión u realiza otras acciones necesarias
        string message1 = "fmt";
        byte[] data1 = Encoding.UTF8.GetBytes(message1);
        tcpStream.Write(data1, 0, data1.Length);
        Debug.Log("Finalizar guardado");
        guardando = false;

        byte[] buffercontrol1 = new byte[1024];
        int bytesRecived1 = tcpStream.Read(buffercontrol1, 0, buffercontrol1.Length);
        transfiriendo.SetActive(false);
        FinalizarConexion();
    }


    public void Punto_Inicial_Trayectoria()
    {

        if (ActiveC == true && modo == 2)
        {
            string message = "mti";
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpStream.Write(data, 0, data.Length);
            Debug.Log("Posicionando punto inicial trayectoria");
            byte[] buffercontrol = new byte[1024];
            int bytesRecived = tcpStream.Read(buffercontrol, 0, buffercontrol.Length);

        }
        else
        {
            Debug.Log("El robot no esta conectado");
        }
    }

    public void Iniciar_Trayectoria()
    {

        if (ActiveC == true && modo == 2)
        {
            string message = "mtc";
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpStream.Write(data, 0, data.Length);
            Debug.Log("Posicionando punto inicial trayectoria");
            byte[] buffercontrol = new byte[1024];
            int bytesRecived = tcpStream.Read(buffercontrol, 0, buffercontrol.Length);

        }
        else
        {
            Debug.Log("El robot no esta conectado");
        }
    }

    public void WritePoint()
    {
        GameObject dropdownManagerObject = GameObject.Find("Puntos_irb1090");

        // Obtener una referencia al script TMP_DropdownManager
        dropdownManager = dropdownManagerObject.GetComponent<TMP_DropdownManager_irb1090>();

        // Verificar si la referencia es nula
        if (dropdownManager == null)
        {
            Debug.LogError("No hay puntos disponibles");
            return;
        }

        // Verificar si el archivo ya existe, y si no, crearlo
        if (!File.Exists(rutaCompletapunto))
        {
            File.Create(rutaCompletapunto).Close();
        }

        // Escribir los datos en el archivo de texto
        using (StreamWriter writer = new StreamWriter(rutaCompletapunto))
        {
            writer.WriteLine("Puntos");
            foreach (TMP_DropdownManager_irb1090.DropdownOption option in dropdownManager.options)
            {

                float J1 = option.value1;
                float J2 = option.value2;
                float J3 = option.value3;
                float J4 = option.value4;
                float J5 = option.value5;
                float J6 = option.value6;

                // Crear el mensaje UDP con los ?ngulos y enviarlo al servidor
                string CoordenateMessage = string.Format("{0};{1};{2};{3};{4};{5}", J1, J2, J3, J4, J5, J6);
                writer.WriteLine(CoordenateMessage);

            }

        }

        Debug.Log("Datos guardados en el archivo: " + rutaCompletapunto);
    }



    public void ReadPoint()
    {
        // Verificar si el archivo existe
        if (!File.Exists(rutaCompletapunto))
        {
            Debug.LogError("El archivo no existe: " + rutaCompletapunto);
            return;
        }

        GameObject dropdownManagerObject = GameObject.Find("Puntos_irb1090");

        // Obtener una referencia al script TMP_DropdownManager
        if (dropdownManagerObject != null)
        {
            // Obtener una referencia al script TMP_DropdownManager
            dropdownManager = dropdownManagerObject.GetComponent<TMP_DropdownManager_irb1090>();

            // Verificar si el componente TMP_DropdownManager_schunk fue encontrado
            if (dropdownManager != null)
            {
                // Limpiar las opciones existentes
                dropdownManager.options.Clear();
            }
            else
            {
                Debug.LogError("El objeto 'Puntos' no tiene el componente TMP_DropdownManager_schunk.");
            }
        }
        else
        {
            Debug.LogError("No se encontró un objeto llamado 'Puntos'.");
        }

        // Leer los datos del archivo de texto
        string[] lines = File.ReadAllLines(rutaCompletapunto);

        // Verificar si hay datos en el archivo
        if (lines.Length <= 1)
        {
            Debug.LogError("El archivo está vacío o no tiene el formato esperado.");
            dropdownManager.options.Add(new TMP_DropdownManager_irb1090.DropdownOption("Home ", 0, 0, 0, 0, 0, 0));
            return;
        }

        // Leer cada línea del archivo y agregar los puntos al dropdownManager
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(';');

            if (values.Length != 6)
            {
                Debug.LogError("La línea " + i + " del archivo no tiene el formato esperado.");
                continue;
            }

            float J1, J2, J3, J4, J5, J6;

            if (!float.TryParse(values[0], out J1) ||
                !float.TryParse(values[1], out J2) ||
                !float.TryParse(values[2], out J3) ||
                !float.TryParse(values[3], out J4) ||
                !float.TryParse(values[4], out J5) ||
                !float.TryParse(values[5], out J6))
            {
                Debug.LogError("Error al convertir los valores en la línea " + i + " del archivo.");
                continue;
            }

            if (i == 1)
            {
                dropdownManager.options.Add(new TMP_DropdownManager_irb1090.DropdownOption("Home ", J1, J2, J3, J4, J5, J6));
            }
            else
            {
                dropdownManager.options.Add(new TMP_DropdownManager_irb1090.DropdownOption("Punto " + (i - 1), J1, J2, J3, J4, J5, J6));
            }
            // Agregar la opción al dropdownManager

        }

        // Actualizar las opciones del TMP_Dropdown
        dropdownManager.RefreshDropdownOptions();

        Debug.Log("Datos leídos del archivo: " + rutaCompletapunto);
    }

    public void WriteTrajectory()
    {


        GameObject trayectoriasObject = GameObject.Find("Trayectorias_irb1090");

        // Obtiene una referencia al script Control_trayectoria desde el GameObject
        Control_trayectoria_irb1090 controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_irb1090>();

        // Accede a la estructura DropdownOption desde el script Control_trayectoria
        List<Control_trayectoria_irb1090.DropdownOption> opciones = controlTrayectoriaScript.options;

        if (!File.Exists(rutaCompletatrayectoria))
        {
            File.Create(rutaCompletatrayectoria).Close();
        }


        using (StreamWriter writer = new StreamWriter(rutaCompletatrayectoria))
        {
            //writer.WriteLine("Trayectorias");

            foreach (Control_trayectoria_irb1090.DropdownOption opcion in opciones)
            {
                writer.WriteLine(opcion.name); // Escribir el nombre de la trayectoria

                // Accede a los puntos de la trayectoria de la opción
                foreach (float[] punto in opcion.puntos)
                {

                    string pointLine = string.Format("{0};{1};{2};{3};{4};{5}",
                       punto[0], punto[1], punto[2],
                       punto[3], punto[4], punto[5]);
                    writer.WriteLine(pointLine);
                    // Trabaja con cada punto de la trayectoria
                    Debug.Log("Punto en la trayectoria: " + punto[0] + ", " + punto[1] + ", " + punto[2] + ", " + punto[3] + ", " + punto[4] + ", " + punto[5]);
                }
                // Separador entre trayectorias
                writer.WriteLine("---");
            }
        }
        // Debug.Log("Datos de trayectorias guardados en el archivo: " + fileTrajectory);
    }

    public void LoadTrajectories()
    {
        GameObject trayectoriasObject = GameObject.Find("Trayectorias_irb1090");
        Control_trayectoria_irb1090 controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_irb1090>();

        if (File.Exists(rutaCompletatrayectoria))
        {
            // Limpiar las opciones existentes
            controlTrayectoriaScript.options.Clear();

            // Lista para almacenar las opciones de trayectoria
            List<Control_trayectoria_irb1090.DropdownOption> opciones = new List<Control_trayectoria_irb1090.DropdownOption>();

            // Leer todas las líneas del archivo
            string[] lines = File.ReadAllLines(rutaCompletatrayectoria);

            string currentTrayectoriaName = "";
            List<float[]> currentTrayectoriaPoints = new List<float[]>();

            foreach (string line in lines)
            {
                if (line == "---")
                {
                    // Al encontrar "---", crea una nueva opción de trayectoria y resetea los puntos
                    Control_trayectoria_irb1090.DropdownOption opcion = new Control_trayectoria_irb1090.DropdownOption(currentTrayectoriaName, currentTrayectoriaPoints, new List<float>());
                    opciones.Add(opcion);

                    // Resetear para la siguiente trayectoria
                    currentTrayectoriaName = "";
                    currentTrayectoriaPoints = new List<float[]>();
                }
                else if (currentTrayectoriaName == "")
                {
                    // Si currentTrayectoriaName está vacío, esta línea es el nombre de la nueva trayectoria
                    currentTrayectoriaName = line;
                }
                else
                {
                    // De lo contrario, esta línea es un punto en la trayectoria actual
                    string[] values = line.Split(';');
                    if (values.Length == 6)
                    {
                        float[] punto = new float[6];
                        for (int i = 0; i < 6; i++)
                        {
                            float.TryParse(values[i], out punto[i]);
                        }
                        currentTrayectoriaPoints.Add(punto);
                    }
                    else
                    {
                        Debug.LogError("Línea de punto inválida: " + line);
                    }
                }
            }

            // Asegurarse de agregar la última trayectoria si el archivo no termina con "---"
            if (!string.IsNullOrEmpty(currentTrayectoriaName))
            {
                Control_trayectoria_irb1090.DropdownOption opcion = new Control_trayectoria_irb1090.DropdownOption(currentTrayectoriaName, currentTrayectoriaPoints, new List<float>());
                opciones.Add(opcion);
            }

            // Asigna las opciones de trayectoria al script Control_trayectoria
            controlTrayectoriaScript.options = opciones;

            // Actualiza las opciones del TMP_Dropdown
            controlTrayectoriaScript.RefreshDropdownOptions();
        }
        else
        {
            Debug.LogWarning("El archivo de trayectorias no existe.");
        }
    }


    private void OnApplicationQuit()
    {
        StopServer();
    }

    public void StopServer()
    {
        if (tcpListener != null)
        {
            tcpListener.Stop();
            tcpListener = null;
        }

        if (connectedTcpClient != null)
        {
            connectedTcpClient.Close();
            connectedTcpClient = null;
        }

        Debug.Log("Servidor TCP detenido.");
    }

    public void Envio_datos_programa()
    {
        // Enviar todos los puntos y trayectorias

        envioCompleto = false;
        if (ActiveC == true)
        {
            transfiriendo2.SetActive(true);

            guardando = true;
            string message = "envio_puntos";
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpStream.Write(data, 0, data.Length);
            byte[] buffercontrol = new byte[1024];
            int bytesRecived = tcpStream.Read(buffercontrol, 0, buffercontrol.Length);

            string message_p = dropdownManager.options.Count.ToString();
            byte[] data2 = Encoding.UTF8.GetBytes(message_p);
            tcpStream.Write(data2, 0, data2.Length);
            byte[] buffercontrol2 = new byte[1024];
            int bytesRecived2 = tcpStream.Read(buffercontrol2, 0, buffercontrol2.Length);

            StartCoroutine(EnviarDatosSecuencial());



        }

        // Enviar el programa
    }

    public IEnumerator SendSelectedPointprograma()
    {
        // Obtener el índice del punto seleccionado en el dropdown
        yield return new WaitForSeconds(tiempo_envio);
        int i = 0;
        while (i < dropdownManager.options.Count)
        {
            TMP_DropdownManager_irb1090.DropdownOption selectedOption = dropdownManager.options[i];
            float J1 = selectedOption.value1;
            float J2 = selectedOption.value2;
            float J3 = selectedOption.value3;
            float J4 = selectedOption.value4;
            float J5 = selectedOption.value5;
            float J6 = selectedOption.value6;

            // Crear el mensaje UDP con los ángulos y enviarlo al servidor
            string CoordenateMessage = string.Format("{0};{1};{2};{3};{4};{5}", J1, J2, J3, J4, J5, J6);
            byte[] CoordenateData = Encoding.UTF8.GetBytes(CoordenateMessage);


            //Debug.Log("Valor J1: " + J1);

            tcpStream.Write(CoordenateData, 0, CoordenateData.Length);

            Debug.Log("Punto enviado  " + CoordenateMessage);

            byte[] buffercontrol = new byte[1024];
            int bytesReceived = tcpStream.Read(buffercontrol, 0, buffercontrol.Length);

            i = i + 1;
            yield return new WaitForSeconds(tiempo_envio / 10);
        }

        yield return new WaitForSeconds(tiempo_envio);
        envioCompleto = true;
    }

    private IEnumerator EsperarEnvioCompletopuntos()
    {
        // Espera hasta que el envío esté completo
        yield return new WaitUntil(() => envioCompleto);

    }

    public IEnumerator SendTrajectories()
    {

        GameObject trayectoriasObject = GameObject.Find("Trayectorias_irb1090");
        Control_trayectoria_irb1090 controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_irb1090>();

        if (trayectoriasObject != null)
        {
            controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_irb1090>();
            if (controlTrayectoriaScript == null)
            {
                Debug.LogError("No se encontró el script Control_trayectoria en el GameObject 'Trayectorias'");
            }

        }
        float tiempo = controlTrayectoriaScript.tiempoponderado;
        // Obtener el índice de la trayectoria seleccionada en el dropdown

        string messagec = controlTrayectoriaScript.options.Count.ToString();
        byte[] datac = Encoding.UTF8.GetBytes(messagec);
        tcpStream.Write(datac, 0, datac.Length);
        byte[] buffercontrolc = new byte[1024];
        int bytesRecivedc = tcpStream.Read(buffercontrolc, 0, buffercontrolc.Length);


        int selectedIndex;
        int i = 0;
        while (i < controlTrayectoriaScript.options.Count)
        {
            selectedIndex = i;

            // Obtener la trayectoria seleccionada

            Control_trayectoria_irb1090.DropdownOption selectedTrajectory = controlTrayectoriaScript.options[selectedIndex];

            // Obtener la lista de puntos de la trayectoria seleccionada

            // Crear el mensaje inicial con el número de puntos


            // Enviar el mensaje inicial con el número de puntos


            if (selectedTrajectory.puntos.Count > 0)
            {
                List<float[]> puntos = selectedTrajectory.puntos;
                string numberOfPointsMessage = puntos.Count.ToString();
                byte[] numberOfPointsData = Encoding.UTF8.GetBytes(numberOfPointsMessage);
                tcpStream.Write(numberOfPointsData, 0, numberOfPointsData.Length);
                byte[] buffercontrol = new byte[1024];
                int bytesRecived = tcpStream.Read(buffercontrol, 0, buffercontrol.Length);

                string message = tiempo.ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);
                tcpStream.Write(data, 0, data.Length);

                byte[] buffercontrol2 = new byte[1024];
                int bytesRecived2 = tcpStream.Read(buffercontrol2, 0, buffercontrol2.Length);

                // Esperar un tiempo antes de continuar, si es necesario
                yield return new WaitForSeconds(tiempo_envio);

                // Iterar sobre los puntos de la trayectoria y enviar cada punto
                int k = 0;
                foreach (float[] punto in puntos)
                {

                    // Crear el mensaje UDP con las coordenadas del punto y enviarlo al servidor
                    string CoordenateMessage = string.Format("{0};{1};{2};{3};{4};{5}",
                        punto[0], punto[1], punto[2], punto[3], punto[4], punto[5]);
                    byte[] CoordenateData = Encoding.UTF8.GetBytes(CoordenateMessage);

                    tcpStream.Write(CoordenateData, 0, CoordenateData.Length);

                    Debug.Log("Punto enviado: " + CoordenateMessage);

                    byte[] buffer = new byte[1024];
                    int bytesRead = tcpStream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    response = response.Replace(".", ",");
                    float.TryParse(response, out distancias[i, k]);


                    if (distancias[i, k] > 0)
                    {
                        velocidad = distancias[i, k] / (19 / (scriptTrayectoria.velocidad));
                    }
                    else
                    {
                        velocidad = 190;
                    }


                    if (Limite_velocidad.isOn)
                    {
                        if (velocidad > 190)
                        {
                            velocidades_p[i, k] = 190;
                        }
                    }
                    //velocidades[i] = velocidad;
                    Debug.Log("Distancia: " + distancias[i, k]);
                    // Debug.Log("Velocidad: " + velociades[i]);
                    string message2 = velocidad.ToString();
                    message2 = message2.Replace(',', '.');
                    byte[] data2 = Encoding.UTF8.GetBytes(message2);
                    tcpStream.Write(data2, 0, data2.Length);

                    byte[] buffercontrol3 = new byte[1024];
                    int bytesRecived3 = tcpStream.Read(buffercontrol3, 0, buffercontrol3.Length);

                    // Esperar un tiempo antes de continuar, si es necesarioº
                    yield return new WaitForSeconds(tiempo_envio / 10);
                    k = k + 1;
                }


            }
            else
            {
                string numberOfPointsMessage2 = 0.ToString();
                byte[] numberOfPointsData2 = Encoding.UTF8.GetBytes(numberOfPointsMessage2);
                tcpStream.Write(numberOfPointsData2, 0, numberOfPointsData2.Length);
                byte[] buffercontrol2 = new byte[1024];
                int bytesRecived2 = tcpStream.Read(buffercontrol2, 0, buffercontrol2.Length);
            }
            i = i + 1;
        }


        yield return new WaitForSeconds(tiempo_envio);
        envioCompleto2 = true;
    }


    private IEnumerator EsperarEnvioCompletoTs()
    {
        // Espera hasta que el envío esté completo
        yield return new WaitUntil(() => envioCompleto2);

        // Envío completo, finaliza la conexión u realiza otras acciones necesarias
        string message1 = "fin_envio";
        byte[] data1 = Encoding.UTF8.GetBytes(message1);
        tcpStream.Write(data1, 0, data1.Length);
        guardando = false;

        byte[] buffercontrol1 = new byte[1024];
        int bytesRecived1 = tcpStream.Read(buffercontrol1, 0, buffercontrol1.Length);
        transfiriendo2.SetActive(false);
        FinalizarConexion();
    }

    private IEnumerator EnviarDatosSecuencial()
    {
        yield return StartCoroutine(SendSelectedPointprograma());
        yield return StartCoroutine(EsperarEnvioCompletopuntos());

        envioCompleto = false;
        guardando = true;
        string message3 = "envio_trayectorias";
        byte[] data3 = Encoding.UTF8.GetBytes(message3);
        tcpStream.Write(data3, 0, data3.Length);
        Debug.Log("Inicio guardado trayectoria");
        byte[] buffercontrol3 = new byte[1024];
        int bytesRecived3 = tcpStream.Read(buffercontrol3, 0, buffercontrol3.Length);

        yield return StartCoroutine(SendTrajectories());
        yield return StartCoroutine(EsperarEnvioCompletoTs());


    }

    public IEnumerator Envio_programa(List<Control_programacion_irb1090.Condicion> condiciones)
    {
        string message_control = "Envio_programa";
        byte[] data_control = Encoding.UTF8.GetBytes(message_control);
        tcpStream.Write(data_control, 0, data_control.Length);
        byte[] buffercontrol_control = new byte[1024];
        int bytesRecived_control = tcpStream.Read(buffercontrol_control, 0, buffercontrol_control.Length);

        string message0 = condiciones.Count.ToString();
        byte[] data0 = Encoding.UTF8.GetBytes(message0);
        tcpStream.Write(data0, 0, data0.Length);
        byte[] buffercontrol0 = new byte[1024];
        int bytesRecived0 = tcpStream.Read(buffercontrol0, 0, buffercontrol0.Length);



        int i = 0;
        while (i < condiciones.Count)
        {
            yield return new WaitForSeconds(0.2f);

            string message = condiciones[i].selectedCondicionIndex.ToString();
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpStream.Write(data, 0, data.Length);
            byte[] buffercontrol = new byte[1024];
            int bytesRecived = tcpStream.Read(buffercontrol, 0, buffercontrol.Length);

            string message1 = condiciones[i].x.ToString();
            byte[] data1 = Encoding.UTF8.GetBytes(message1);
            tcpStream.Write(data1, 0, data1.Length);
            byte[] buffercontrol1 = new byte[1024];
            int bytesRecived1 = tcpStream.Read(buffercontrol1, 0, buffercontrol1.Length);

            string message2 = condiciones[i].y.ToString();
            byte[] data2 = Encoding.UTF8.GetBytes(message2);
            tcpStream.Write(data2, 0, data2.Length);
            byte[] buffercontrol2 = new byte[1024];
            int bytesRecived2 = tcpStream.Read(buffercontrol2, 0, buffercontrol2.Length);

            string message_acciones = condiciones[i].acciones.Count.ToString();
            byte[] data_acciones = Encoding.UTF8.GetBytes(message_acciones);
            tcpStream.Write(data_acciones, 0, data_acciones.Length);
            byte[] buffercontrol_acciones = new byte[1024];
            int bytesRecived_acciones = tcpStream.Read(buffercontrol_acciones, 0, buffercontrol_acciones.Length);

            yield return new WaitForSeconds(0.2f);

            int j = 0;
            while (j < condiciones[i].acciones.Count)
            {

                string message3 = condiciones[i].acciones[j].tipo_accion.ToString();
                byte[] data3 = Encoding.UTF8.GetBytes(message3);
                tcpStream.Write(data3, 0, data3.Length);
                byte[] buffercontrol3 = new byte[1024];
                int bytesRecived3 = tcpStream.Read(buffercontrol3, 0, buffercontrol3.Length);

                string message4 = condiciones[i].acciones[j].numero_accion.ToString();
                byte[] data4 = Encoding.UTF8.GetBytes(message4);
                tcpStream.Write(data4, 0, data4.Length);
                byte[] buffercontrol4 = new byte[1024];
                int bytesRecived4 = tcpStream.Read(buffercontrol4, 0, buffercontrol4.Length);

                j = j + 1;
            }

            i = i + 1;
        }
        yield return new WaitForSeconds(0.2f);


    }

    public IEnumerator comprobar_entradas(List<Toggle> entradas)
    {

        if (entradas[0].isOn == true)
        {
            string entrada1 = "1";
            byte[] data1 = Encoding.UTF8.GetBytes(entrada1);
            tcpStream.Write(data1, 0, data1.Length);
            byte[] buffercontrol1 = new byte[1024];
            int bytesRecived1 = tcpStream.Read(buffercontrol1, 0, buffercontrol1.Length);
        }
        else
        {
            string entrada1 = "0";
            byte[] data1 = Encoding.UTF8.GetBytes(entrada1);
            tcpStream.Write(data1, 0, data1.Length);
            byte[] buffercontrol1 = new byte[1024];
            int bytesRecived1 = tcpStream.Read(buffercontrol1, 0, buffercontrol1.Length);
        }
        if (entradas[1].isOn == true)
        {
            string entrada2 = "1";
            byte[] data2 = Encoding.UTF8.GetBytes(entrada2);
            tcpStream.Write(data2, 0, data2.Length);
            byte[] buffercontrol2 = new byte[1024];
            int bytesRecived2 = tcpStream.Read(buffercontrol2, 0, buffercontrol2.Length);
        }
        else
        {
            string entrada2 = "0";
            byte[] data2 = Encoding.UTF8.GetBytes(entrada2);
            tcpStream.Write(data2, 0, data2.Length);
            byte[] buffercontrol2 = new byte[1024];
            int bytesRecived2 = tcpStream.Read(buffercontrol2, 0, buffercontrol2.Length);
        }
        if (entradas[2].isOn == true)
        {
            string entrada3 = "1";
            byte[] data3 = Encoding.UTF8.GetBytes(entrada3);
            tcpStream.Write(data3, 0, data3.Length);
            byte[] buffercontrol3 = new byte[1024];
            int bytesRecived3 = tcpStream.Read(buffercontrol3, 0, buffercontrol3.Length);
        }
        else
        {
            string entrada3 = "0";
            byte[] data3 = Encoding.UTF8.GetBytes(entrada3);
            tcpStream.Write(data3, 0, data3.Length);
            byte[] buffercontrol3 = new byte[1024];
            int bytesRecived3 = tcpStream.Read(buffercontrol3, 0, buffercontrol3.Length);
        }
        if (entradas[3].isOn == true)
        {
            string entrada4 = "1";
            byte[] data4 = Encoding.UTF8.GetBytes(entrada4);
            tcpStream.Write(data4, 0, data4.Length);
            byte[] buffercontrol4 = new byte[1024];
            int bytesRecived4 = tcpStream.Read(buffercontrol4, 0, buffercontrol4.Length);
        }
        else
        {
            string entrada4 = "0";
            byte[] data4 = Encoding.UTF8.GetBytes(entrada4);
            tcpStream.Write(data4, 0, data4.Length);
            byte[] buffercontrol4 = new byte[1024];
            int bytesRecived4 = tcpStream.Read(buffercontrol4, 0, buffercontrol4.Length);
        }
        if (entradas[4].isOn == true)
        {
            string entrada5 = "1";
            byte[] data5 = Encoding.UTF8.GetBytes(entrada5);
            tcpStream.Write(data5, 0, data5.Length);
            byte[] buffercontrol5 = new byte[1024];
            int bytesRecived5 = tcpStream.Read(buffercontrol5, 0, buffercontrol5.Length);
        }
        else
        {
            string entrada5 = "0";
            byte[] data5 = Encoding.UTF8.GetBytes(entrada5);
            tcpStream.Write(data5, 0, data5.Length);
            byte[] buffercontrol5 = new byte[1024];
            int bytesRecived5 = tcpStream.Read(buffercontrol5, 0, buffercontrol5.Length);
        }

        yield return new WaitForSeconds(0.2f);
    }
}