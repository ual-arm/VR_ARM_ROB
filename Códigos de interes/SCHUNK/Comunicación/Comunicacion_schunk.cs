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


public class Comunicacion_schunk : MonoBehaviour
{
    // Start is called before the first frame update

    public string robotStudioIP = "127.0.0.1";                        // IP cuando RobotStudio y Unity en distinto ordenador
    public int robotStudioTcpPort = 12000;                            // IP cuando RobotStudio y Unity en mismo ordenador
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

    private TMP_DropdownManager_schunk dropdownManager;
    public Control_trayectoria_schunk scriptTrayectoria;
    private TMP_Dropdown dropdownManager2;

    private bool guardando = false;

    private bool cargando = false;

    private bool envioCompleto = false;
    private bool envioCompleto2 = false;


    private string nombreArchivopuntos = "Puntos_schunk.txt";
    private string nombreArchivotrayectoria = "Trayectorias_schunk.txt";

    string directorioProyecto;
    string rutaCompletapunto;
    string rutaCompletatrayectoria;

    public GameObject transfiriendo;
    public GameObject transfiriendo2;

    public GameObject programacion;
    private Control_programacion2_schunk scriptprogramacion;

    private TcpClient connectedTcpClient;

    public List<Control_programacion2_schunk.Condicion> listaCondiciones = new List<Control_programacion2_schunk.Condicion>();

    public Toggle Limite_velocidad;
    public GameObject Limite_velocidad_;

    public float[] distancia = new float[1000];
    public float[,] distancias = new float[20, 1000];
    public float[] velocidades = new float[1000];
    public float[,] velocidades_p = new float[20, 1000];
    //public float[] tiempo_ = new float[1000];

    static float L1 = 0.311f, L2 = 0.4f, L3 = 0.39f, L4 = 0.39f, L5 = 0.078f, L6 = 0.1f;


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

    // Se cargan los datos de puntos y trayectorias y se referencian los scrip

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
        scriptprogramacion = programacion.GetComponent<Control_programacion2_schunk>();
        scriptprogramacion.LoadPrograma();
    }

    // Se actualiza al cambiar desplegable para comprobar si hay comunicación

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

    // Establece la comunicación con servidor y en función del valor del desplegable realiza envia una información u otra

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
                byte[] bufferq = new byte[1024];
                int bytesReadq = tcpStream.Read(bufferq, 0, bufferq.Length);
                string rawResponse = Encoding.UTF8.GetString(bufferq, 0, bytesReadq);
                string response = rawResponse.Trim();
                //string response = Encoding.UTF8.GetString(bufferq, 0, bytesReadq);

                Debug.Log("Mensaje recibido"+ response);

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
                        Limite_velocidad_.SetActive(false);
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
                    Limite_velocidad_.SetActive(false);
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

    // Procedimiento por si queremos que Unity actue como servidor y robotstudio como cliente, habria que cambiar tambien el codigo de robotstudio

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
                    Limite_velocidad_.SetActive(false);
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
                Limite_velocidad_.SetActive(false);
                Envio_datos_programa();
            }

        }

        //yield return null;

    }

    // Gestiona el intercambio de información inicial en caso de que Unity actue como servidor.

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


    // Finaliza la conexión, en caso de que existan sockets los cierra.
    public void FinalizarConexiones()
    {
        Debug.Log("Finalizando conexiones.");

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

    // Finaliza la conexión de forma no permanente, es decir detien el envio de información pero no cierra los sockets:

    public void FinalizarConexion()
    {
        Debug.Log("Finalizando conexion.");

        loopsend = false;
        //guardando = false;

    }

    // Funcion para transmitir el movimiento del robot en directo:

    IEnumerator EjecutarCadaSegundo()
    {
        float tiempo_calculado;
        float tiempo;

        string message = "m";
        byte[] data = Encoding.UTF8.GetBytes(message);
        tcpStream.Write(data, 0, data.Length);
        //while (loopsend == true&& modo==0)
        byte[] buffercontrol1 = new byte[1024];
        int bytesRecived1 = tcpStream.Read(buffercontrol1, 0, buffercontrol1.Length);

        float J1 = Articulacion1.localEulerAngles.z;
        float J2 = Articulacion2.localEulerAngles.y;
        float J3 = Articulacion3.localEulerAngles.y;
        float J4 = Articulacion4.localEulerAngles.y;
        float J5 = Articulacion5.localEulerAngles.y;
        float J6 = Articulacion5.localEulerAngles.y;

        float j1 = J1;
        float j2 = J2;
        float j3 = J3;
        float j4 = J4;
        float j5 = J5;
        float j6 = J6;



        // Crear el mensaje UDP con los ?ngulos y enviarlo al servidor
        string CoordenateMessagei = string.Format("{0};{1};{2};{3};{4};{5};", J1, J2, J3, J4, J5, J6);
        CoordenateMessagei = CoordenateMessagei.Replace(',', '.');
        byte[] CoordenateDatai = Encoding.UTF8.GetBytes(CoordenateMessagei);

        tcpStream.Write(CoordenateDatai, 0, CoordenateDatai.Length);

        byte[] buffercontroli = new byte[1024];
        int bytesRecivedi = tcpStream.Read(buffercontroli, 0, buffercontroli.Length);

        while (modo == 0 && loopsend == true)
        {

            //J1 = Articulacion1.localEulerAngles.z;
            //J2 = Articulacion2.localEulerAngles.y;
            //J3 = Articulacion3.localEulerAngles.y;
            //J4 = Articulacion4.localEulerAngles.x;
            //J5 = Articulacion5.localEulerAngles.y;
            //J6 = Articulacion5.localEulerAngles.y;

            // quizas lo de abajo lo pueda quitar para ganar tiempo

            //tiempo_calculado= Calcular_Distancia(j1, j2, j3, j4, j5, j6, J1, J2, J3, J4, J5, J6)/2;
            //if (tiempo_calculado > 0.4)
            //{
            //    tiempo = tiempo_calculado;
            //}
            //else
            //{
            //    tiempo = 0.4f;
            //}




            Quaternion rotation1 = Articulacion1.localRotation;
            rotation1.ToAngleAxis(out angulo1, out eje1);

            Quaternion rotation2 = Articulacion2.localRotation;
            rotation2.ToAngleAxis(out angulo2, out eje2);

            Quaternion rotation3 = (Articulacion3.localRotation);
            rotation3.ToAngleAxis(out angulo3, out eje3);

            //angulo3 = angulo3 - 90;
            Quaternion rotation4 = Articulacion4.localRotation;
            rotation4.ToAngleAxis(out angulo4, out eje4);

            Quaternion rotation5 = Articulacion5.localRotation;
            rotation5.ToAngleAxis(out angulo5, out eje5);

            //angulo5 = angulo5 - 90;
            Quaternion rotation6 = Articulacion6.localRotation;
            rotation6.ToAngleAxis(out angulo6, out eje6);


            if (eje1.z > 0)
            {
                angulo1 = -angulo1;
            }
            if (eje2.y > 0)
            {
                angulo2 = -angulo2;
            }
            if (eje3.y > 0)
            {
                angulo3 = -angulo3;
            }
            if (eje4.x < 0)
            {
                angulo4 = -angulo4;
            }
            if (eje5.y > 0)
            {
                angulo5 = -angulo5;
            }
            if (eje6.x < 0)
            {
                angulo6 = -angulo6;
            }


            //string tiempo_ = tiempo.ToString();
            //tiempo_ = tiempo_.Replace(',', '.');
            //byte[] data_ = Encoding.UTF8.GetBytes(tiempo_);
            //tcpStream.Write(data_, 0, data_.Length);
            //while (loopsend == true&& modo==0)
            //byte[] buffercontrol_ = new byte[1024];
            //int bytesRecived_ = tcpStream.Read(buffercontrol_, 0, buffercontrol_.Length);

            // Crear el mensaje UDP con los ?ngulos y enviarlo al servidor
            string CoordenateMessage = string.Format("{0};{1};{2};{3};{4};{5};", angulo1, angulo2, angulo3, angulo4, -angulo5, -angulo6);
            CoordenateMessage = CoordenateMessage.Replace(',', '.');
            byte[] CoordenateData = Encoding.UTF8.GetBytes(CoordenateMessage);

            tcpStream.Write(CoordenateData, 0, CoordenateData.Length);

            //byte[] buffercontrol2 = new byte[1024];
            //int bytesRecived2 = tcpStream.Read(buffercontrol2, 0, buffercontrol2.Length);

            //yield return new WaitForSeconds(tiempo_envio);
            yield return new WaitForSeconds(0.4f);

            //j1 = J1;
            //j2 = J2;
            //j3 = J3;
            //j4 = J4;
            //j5 = J5;
            //j6 = J6;

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


    // Función para indicar a Robotstudio que queremos mover a un punto

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
    // Función para procesar los movimientos necesarios para alcanzar el punto y enviarlos a robotstudio

    public IEnumerator SendSelectedPoint()
    {

        GameObject dropdownManagerObject = GameObject.Find("Puntos_schunk");

        // Obtener una referencia al script TMP_DropdownManager
        if (dropdownManagerObject != null)
        {
            // Obtener una referencia al script TMP_DropdownManager
            dropdownManager = dropdownManagerObject.GetComponent<TMP_DropdownManager_schunk>();

            // Verificar si el componente TMP_DropdownManager_schunk fue encontrado
            if (dropdownManager != null)
            {
                // Limpiar las opciones existentes
                //dropdownManager.options.Clear();
                
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

        // Obtener el índice del punto seleccionado en el dropdown
        int selectedIndex = dropdown_puntos.value;
        float tiempo;
        float velocidad_;

        // Verificar que el índice sea válido
        if (selectedIndex < 0 )
        {
            Debug.LogError("Índice de selección fuera de rango.");
           yield break;
        }

        


        // Obtener la opción seleccionada
        TMP_DropdownManager_schunk.DropdownOption selectedOption = dropdownManager.options[selectedIndex];

        // Obtener los valores de los ángulos del punto seleccionado
        float J1 = selectedOption.value1;
        float J2 = selectedOption.value2;
        float J3 = selectedOption.value3;
        float J4 = selectedOption.value4;
        float J5 = selectedOption.value5;
        float J6 = selectedOption.value6;

        float j1 = Articulacion1.localEulerAngles.z;
        float j2 = Articulacion2.localEulerAngles.y;
        float j3 = Articulacion3.localEulerAngles.y;
        float j4 = Articulacion4.localEulerAngles.y;
        float j5 = Articulacion5.localEulerAngles.y;
        float j6 = Articulacion5.localEulerAngles.y;

        distancia[0] = Calcular_Distancia(j1, j2, j3, j4, j5, j6, J1, J2, J3, J4, J5, J6);
        velocidad_ = distancia[0] / velocidad;
        if (velocidad_ < 2)
        {
            tiempo = velocidad;
        }
        else
        {
            tiempo = distancia[0] / 2;
        }

       // Debug.LogError("Punto seleccionado: " + tiempo);
       // Debug.LogError("Punto lomite seleccionado: " + distancia[0]);

        string tiempo_ = tiempo.ToString();
        tiempo_ = tiempo_.Replace(',', '.');
        byte[] data_ = Encoding.UTF8.GetBytes(tiempo_);
        tcpStream.Write(data_, 0, data_.Length);
        //while (loopsend == true&& modo==0)
        byte[] buffercontrol_ = new byte[1024];
        int bytesRecived_ = tcpStream.Read(buffercontrol_, 0, buffercontrol_.Length);

        // Crear el mensaje UDP con los ángulos y enviarlo al servidor
        string CoordenateMessage = string.Format("{0};{1};{2};{3};{4};{5};", J1, J2, J3, J4, -J5, J6);
        CoordenateMessage = CoordenateMessage.Replace(',', '.');
        byte[] CoordenateData = Encoding.UTF8.GetBytes(CoordenateMessage);
        tcpStream.Write(CoordenateData, 0, CoordenateData.Length);
        byte[] buffer = new byte[1024];
        int bytesRead = tcpStream.Read(buffer, 0, buffer.Length);

        // Esperar un tiempo antes de continuar, si es necesario
        //yield return new WaitForSeconds(tiempo_envio);
        envioCompleto = true;
    }

    // Espera a que el punto halla sido enviado para que ambos robots se empiecen a mover a la vez.
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
    // Indica a robotstudio que quiere desplazarse a lo largo de una trayectoria

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

    // Se procesan todos los movimientos necesarios para realizar la trayectoria  y se envian a robotstudio

    public IEnumerator SendSelectedTrajectory()
    {

        GameObject trayectoriasObject = GameObject.Find("Trayectorias_schunk");
        Control_trayectoria_schunk controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_schunk>();

        if (trayectoriasObject != null)
        {
            controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_schunk>();
            if (controlTrayectoriaScript == null)
            {
                Debug.LogError("No se encontró el script Control_trayectoria en el GameObject 'Trayectorias'");
            }

        }
        double tiempo = controlTrayectoriaScript.tiempoponderado;
        float velocidadMaxima;
        float tiempo_calculado;

        float tiempo2;


        // Obtener el índice de la trayectoria seleccionada en el dropdown
        int selectedIndex = dropdown_trayectorias.value;
        Debug.Log("El indice es: " + selectedIndex);
        // Verificar que el índice sea válido
        if (selectedIndex < 0 || selectedIndex >= controlTrayectoriaScript.options.Count)
        {
            Debug.LogError("Índice de selección fuera de rango.");
            yield break;
        }

        // Obtener la trayectoria seleccionada
        Control_trayectoria_schunk.DropdownOption selectedTrajectory = controlTrayectoriaScript.options[selectedIndex];

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



        // Esperar un tiempo antes de continuar, si es necesario
        yield return new WaitForSeconds(tiempo_envio);

        // Iterar sobre los puntos de la trayectoria y enviar cada punto
        int i = 0;

        float j1 = Articulacion1.localEulerAngles.z;
        float j2 = Articulacion2.localEulerAngles.y;
        float j3 = Articulacion3.localEulerAngles.y;
        float j4 = Articulacion4.localEulerAngles.y;
        float j5 = Articulacion5.localEulerAngles.y;
        float j6 = Articulacion5.localEulerAngles.y;

        
        foreach (float[] punto in puntos)
        {

            // Crear el mensaje UDP con las coordenadas del punto y enviarlo al servidor
           
            string CoordenateMessage = string.Format("{0};{1};{2};{3};{4};{5};",
                punto[0], punto[1], punto[2], (-punto[3]), (-punto[4]), punto[5]);
            CoordenateMessage = CoordenateMessage.Replace(',', '.');
            byte[] CoordenateData = Encoding.UTF8.GetBytes(CoordenateMessage);

            tcpStream.Write(CoordenateData, 0, CoordenateData.Length);

            Debug.Log("Punto enviado: " + CoordenateMessage);

            byte[] buffer = new byte[1024];
            int bytesRead = tcpStream.Read(buffer, 0, buffer.Length);

            tiempo_calculado = Calcular_Distancia(j1, j2, j3, j4, j5, j6, punto[0], punto[1], punto[2], punto[3], punto[4], punto[5]) / 2;
            tiempo_calculado = 100 * tiempo_calculado / scriptTrayectoria.tiempoEntrePuntos;
            if (Limite_velocidad.isOn)
            {
                if (tiempo_calculado > tiempo_envio)
                {
                    //tiempo_[i] = tiempo_calculado;
                }
                else
                {
                    //tiempo_[i] = scriptTrayectoria.tiempoEntrePuntos;
                }
            }   
            else
            {   
                //tiempo_[i] = scriptTrayectoria.tiempoEntrePuntos;
                tiempo = scriptTrayectoria.tiempoEntrePuntos*1.1f;
            }
             //Debug.LogError("Punto seleccionado: " + tiempo);

            // Debug.Log("Velocidad: " + velociades[i]);
            string message = tiempo.ToString();
            message = message.Replace(',', '.');
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpStream.Write(data, 0, data.Length);

            byte[] buffercontrol2 = new byte[1024];
            int bytesRecived2 = tcpStream.Read(buffercontrol2, 0, buffercontrol2.Length);

            // Esperar un tiempo antes de continuar, si es necesarioº
            yield return new WaitForSeconds(scriptTrayectoria.tiempoEntrePuntos);

            j1 = punto[0];
            j2 = punto[1];
            j3 = punto[2];
            j4 = punto[3];
            j5 = punto[4];
            j6 = punto[5];

            i = i + 1;
        }




        envioCompleto = true;
    }

    // Espera a que la trayectoria sehalla enviado completamente antes de permitir realizar otra acción


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

    // Indica a robotstudio cuando se quiere desplazar al punto inicial de la trayectoria

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

    // Indica a robotstudio cuando se quiere realizar la trayectoria, si antes a alcanzado el punto inicial.
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
    // Permite almacenar los puntos en el archivo de guardado permanente

    public void WritePoint()
    {

        GameObject dropdownManagerObject = GameObject.Find("Puntos_schunk");

        // Obtener una referencia al script TMP_DropdownManager
        dropdownManager = dropdownManagerObject.GetComponent<TMP_DropdownManager_schunk>();

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
            foreach (TMP_DropdownManager_schunk.DropdownOption option in dropdownManager.options)
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

    // Permite cargar los puntos guardados la iniciar el programa

    public void ReadPoint()
    {
        // Verificar si el archivo existe
        if (!File.Exists(rutaCompletapunto))
        {
            Debug.LogError("El archivo no existe: " + rutaCompletapunto);
            return;
        }

        GameObject dropdownManagerObject = GameObject.Find("Puntos_schunk");

        // Obtener una referencia al script TMP_DropdownManager
        if (dropdownManagerObject != null)
        {
            // Obtener una referencia al script TMP_DropdownManager
            dropdownManager = dropdownManagerObject.GetComponent<TMP_DropdownManager_schunk>();

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
            dropdownManager.options.Add(new TMP_DropdownManager_schunk.DropdownOption("Home ", 0, 0, 0, 0, 0, 0));
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
                dropdownManager.options.Add(new TMP_DropdownManager_schunk.DropdownOption("Home ", J1, J2, J3, J4, J5, J6));
            }
            else
            {
                dropdownManager.options.Add(new TMP_DropdownManager_schunk.DropdownOption("Punto " + (i - 1), J1, J2, J3, J4, J5, J6));
            }
            // Agregar la opción al dropdownManager

        }

        // Actualizar las opciones del TMP_Dropdown
        dropdownManager.RefreshDropdownOptions();


        Debug.Log("Datos leídos del archivo: " + rutaCompletapunto);
    }

    // Permite almacenar las trayectorias de una forma permanente

    public void WriteTrajectory()
    {


        GameObject trayectoriasObject = GameObject.Find("Trayectorias_schunk");

        // Obtiene una referencia al script Control_trayectoria desde el GameObject
        Control_trayectoria_schunk controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_schunk>();

        // Accede a la estructura DropdownOption desde el script Control_trayectoria
        List<Control_trayectoria_schunk.DropdownOption> opciones = controlTrayectoriaScript.options;

        if (!File.Exists(rutaCompletatrayectoria))
        {
            File.Create(rutaCompletatrayectoria).Close();
        }


        using (StreamWriter writer = new StreamWriter(rutaCompletatrayectoria))
        {
            //writer.WriteLine("Trayectorias");

            foreach (Control_trayectoria_schunk.DropdownOption opcion in opciones)
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

    // Permite cargar las trayectorias guardadas al iniciar el programa

    public void LoadTrajectories()
    {
        GameObject trayectoriasObject = GameObject.Find("Trayectorias_schunk");
        Control_trayectoria_schunk controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_schunk>();

        if (File.Exists(rutaCompletatrayectoria))
        {
            // Limpiar las opciones existentes
            controlTrayectoriaScript.options.Clear();

            // Lista para almacenar las opciones de trayectoria
            List<Control_trayectoria_schunk.DropdownOption> opciones = new List<Control_trayectoria_schunk.DropdownOption>();

            // Leer todas las líneas del archivo
            string[] lines = File.ReadAllLines(rutaCompletatrayectoria);

            string currentTrayectoriaName = "";
            List<float[]> currentTrayectoriaPoints = new List<float[]>();

            foreach (string line in lines)
            {
                if (line == "---")
                {
                    // Al encontrar "---", crea una nueva opción de trayectoria y resetea los puntos
                    Control_trayectoria_schunk.DropdownOption opcion = new Control_trayectoria_schunk.DropdownOption(currentTrayectoriaName, currentTrayectoriaPoints, new List<float>());
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
                Control_trayectoria_schunk.DropdownOption opcion = new Control_trayectoria_schunk.DropdownOption(currentTrayectoriaName, currentTrayectoriaPoints, new List<float>());
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

    // Se asegura de cerrar la comunicación para que no existan problemas en futuras conexiones
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

    // Avisa a robotstudio de que se van a enviar los programas guardados

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

    // Envia todos los puntos a robotstudio

    public IEnumerator SendSelectedPointprograma()
    {
        // Obtener el índice del punto seleccionado en el dropdown
        yield return new WaitForSeconds(tiempo_envio);
        int i = 0;
        while (i < dropdownManager.options.Count)
        {
            TMP_DropdownManager_schunk.DropdownOption selectedOption = dropdownManager.options[i];
            float J1 = selectedOption.value1;
            float J2 = selectedOption.value2;
            float J3 = selectedOption.value3;
            float J4 = selectedOption.value4;
            float J5 = selectedOption.value5;
            float J6 = selectedOption.value6;

            // Crear el mensaje UDP con los ángulos y enviarlo al servidor
            string CoordenateMessage = string.Format("{0};{1};{2};{3};{4};{5};", J1, J2, J3, J4, -J5, J6);
            CoordenateMessage = CoordenateMessage.Replace(',', '.');
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

    // Envia todas las trayectorias a robotstudio

    public IEnumerator SendTrajectories()
    {
        float tiempo_calculado;

        GameObject trayectoriasObject = GameObject.Find("Trayectorias_schunk");
        Control_trayectoria_schunk controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_schunk>();

        if (trayectoriasObject != null)
        {
            controlTrayectoriaScript = trayectoriasObject.GetComponent<Control_trayectoria_schunk>();
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

            Control_trayectoria_schunk.DropdownOption selectedTrajectory = controlTrayectoriaScript.options[selectedIndex];

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

                // Esperar un tiempo antes de continuar, si es necesario
                yield return new WaitForSeconds(scriptTrayectoria.tiempoEntrePuntos);


                float j1 = Articulacion1.localEulerAngles.z;
                float j2 = Articulacion2.localEulerAngles.y;
                float j3 = Articulacion3.localEulerAngles.y;
                float j4 = Articulacion4.localEulerAngles.y;
                float j5 = Articulacion5.localEulerAngles.y;
                float j6 = Articulacion5.localEulerAngles.y;

                // Iterar sobre los puntos de la trayectoria y enviar cada punto
                int k = 0;
                foreach (float[] punto in puntos)
                {
                    // Crear el mensaje UDP con las coordenadas del punto y enviarlo al servidor
                    string CoordenateMessage = string.Format("{0};{1};{2};{3};{4};{5};",
                        punto[0], punto[1], punto[2], punto[3], -punto[4], punto[5]);
                    CoordenateMessage = CoordenateMessage.Replace(',', '.');
                    byte[] CoordenateData = Encoding.UTF8.GetBytes(CoordenateMessage);

                    tcpStream.Write(CoordenateData, 0, CoordenateData.Length);

                    Debug.Log("Punto enviado: " + CoordenateMessage);

                    byte[] buffer = new byte[1024];
                    int bytesRead = tcpStream.Read(buffer, 0, buffer.Length);


                    tiempo_calculado = Calcular_Distancia(j1, j2, j3, j4, j5, j6, punto[0], punto[1], punto[2], punto[3], punto[4], punto[5]) / 2;
                    tiempo_calculado = 100 * tiempo_calculado / scriptTrayectoria.velocidad;
                    if (Limite_velocidad.isOn)
                    {
                        if (tiempo_calculado > tiempo_envio)
                        {
                           // tiempo_[i] = tiempo_calculado;
                        }
                        else
                        {
                           // tiempo_[i] = tiempo_envio;
                        }
                    }
                    else
                    {
                        tiempo = scriptTrayectoria.tiempoEntrePuntos*1.05f;
                    }

                    // Debug.Log("Velocidad: " + velociades[i]);
                    string message = tiempo.ToString();
                    message = message.Replace(',', '.');
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    tcpStream.Write(data, 0, data.Length);

                    byte[] buffercontrol2 = new byte[1024];
                    int bytesRecived2 = tcpStream.Read(buffercontrol2, 0, buffercontrol2.Length);

                    // Esperar un tiempo antes de continuar, si es necesarioº
                    yield return new WaitForSeconds(scriptTrayectoria.tiempoEntrePuntos / 2);

                    j1 = punto[0];
                    j2 = punto[1];
                    j3 = punto[2];
                    j4 = punto[3];
                    j5 = punto[4];
                    j6 = punto[5];
                    // Esperar un tiempo antes de continuar, si es necesarioº

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


        yield return new WaitForSeconds(scriptTrayectoria.tiempoEntrePuntos);
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

        //byte[] buffercontrol1 = new byte[1024];
        //int bytesRecived1 = tcpStream.Read(buffercontrol1, 0, buffercontrol1.Length);
        transfiriendo2.SetActive(false);
        FinalizarConexion();
    }

    // Inicia el envio de puntos y posteriormente el envio de trayectorias

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

    // Permite enviar el programa e inicializarlo cuando se deseee

    public IEnumerator Envio_programa(List<Control_programacion2_schunk.Condicion> condiciones)
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

    // realiza una comprobación periodica del estado de las entradas digitales

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
            Debug.Log("Entrada1: " + entrada1);
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
            Debug.Log("Entrada2: " + entrada2);
        }


        //if (entradas[2].isOn == true)
        //{
        //    string entrada3 = "1";
        //byte[] data3 = Encoding.UTF8.GetBytes(entrada3);
        //tcpStream.Write(data3, 0, data3.Length);
        //byte[] buffercontrol3 = new byte[1024];
        //int bytesRecived3 = tcpStream.Read(buffercontrol3, 0, buffercontrol3.Length);
        //}
        //else
        //{
        //string entrada3 = "0";
        //byte[] data3 = Encoding.UTF8.GetBytes(entrada3);
        //tcpStream.Write(data3, 0, data3.Length);
        //byte[] buffercontrol3 = new byte[1024];
        //int bytesRecived3 = tcpStream.Read(buffercontrol3, 0, buffercontrol3.Length);
        //Debug.Log("Entrada3: " + entrada3);
        //}


        //if (entradas[3].isOn == true)
        //{
        //string entrada4b = "1";
        //byte[] data4b = Encoding.UTF8.GetBytes(entrada4b);
        //tcpStream.Write(data4b, 0, data4b.Length);
        //byte[] buffercontrol4b = new byte[1024];
        //int bytesRecived4b = tcpStream.Read(buffercontrol4b, 0, buffercontrol4b.Length);
        //}
        //else
        //{
        //string entrada4b = "0";
        //byte[] data4b = Encoding.UTF8.GetBytes(entrada4b);
        //tcpStream.Write(data4b, 0, data4b.Length);
        //byte[] buffercontrol4b = new byte[1024];
        //int bytesRecived4b = tcpStream.Read(buffercontrol4b, 0, buffercontrol4b.Length);
        //Debug.Log("Entrada4: " + entrada4b);
        //}


        //if (entradas[4].isOn == true)
        //{
        //string entrada5 = "1";
        //byte[] data5 = Encoding.UTF8.GetBytes(entrada5);
        //tcpStream.Write(data5, 0, data5.Length);
        //byte[] buffercontrol5 = new byte[1024];
        //int bytesRecived5 = tcpStream.Read(buffercontrol5, 0, buffercontrol5.Length);
        //}
        //else
        //{
        //string entrada5 = "0";
        //byte[] data5 = Encoding.UTF8.GetBytes(entrada5);
        //tcpStream.Write(data5, 0, data5.Length);
        //byte[] buffercontrol5 = new byte[1024];
        //int bytesRecived5 = tcpStream.Read(buffercontrol5, 0, buffercontrol5.Length);
        //Debug.Log("Entrada5: " + entrada5);
        //}


        //yield return new WaitForSeconds(0.2f);
        yield return null;
    }

    float Calcular_Distancia(float valorA1, float valorA2, float valorA3, float valorA4, float valorA5, float valorA6, float valorB1, float valorB2, float valorB3, float valorB4, float valorB5, float valorB6)
    {
        float distancia;
        Vector3 Origen = ForwardKinematics(valorA1, valorA2, valorA3, valorA4, valorA5, valorA6 );
        Vector3 Extremo = ForwardKinematics(valorB1, valorB2, valorB3, valorB4, valorB5, valorB6);

        distancia = Vector3.Distance(Origen, Extremo);

        return distancia;
    }
    Vector3 ForwardKinematics(float theta1, float theta2, float theta3, float theta4, float theta5, float theta6)
    {
        // Convertir ángulos de grados a radianes
        float rad1 = Mathf.Deg2Rad * theta1;
        float rad2 = Mathf.Deg2Rad * theta2;
        float rad3 = Mathf.Deg2Rad * theta3;
        float rad4 = Mathf.Deg2Rad * theta4;
        float rad5 = Mathf.Deg2Rad * theta5;
        float rad6 = Mathf.Deg2Rad * theta6;

        // Calcular la posición del extremo del robot
        float x = L1 * Mathf.Cos(rad1) * Mathf.Cos(rad2 + rad3 + rad4) + L2 * Mathf.Cos(rad1) * Mathf.Sin(rad2 + rad3 + rad4) + L3 * Mathf.Cos(rad1) * Mathf.Sin(rad2 + rad3) + L4 * Mathf.Cos(rad1) * Mathf.Sin(rad2) + L5 * Mathf.Cos(rad1) * Mathf.Sin(rad2) + L6 * Mathf.Cos(rad1) * Mathf.Sin(rad2);
        float y = L1 * Mathf.Sin(rad1) * Mathf.Cos(rad2 + rad3 + rad4) + L2 * Mathf.Sin(rad1) * Mathf.Sin(rad2 + rad3 + rad4) + L3 * Mathf.Sin(rad1) * Mathf.Sin(rad2 + rad3) + L4 * Mathf.Sin(rad1) * Mathf.Sin(rad2) + L5 * Mathf.Sin(rad1) * Mathf.Sin(rad2) + L6 * Mathf.Sin(rad1) * Mathf.Sin(rad2);
        float z = L1 * Mathf.Cos(rad2 + rad3 + rad4) + L2 * Mathf.Sin(rad2 + rad3 + rad4) + L3 * Mathf.Sin(rad2 + rad3) + L4 * Mathf.Sin(rad2) + L5 * Mathf.Sin(rad2) + L6 * Mathf.Sin(rad2);

        return new Vector3(x, y, z);
    }
}
