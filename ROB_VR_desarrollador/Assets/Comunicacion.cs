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

public class Comunicacion : MonoBehaviour
{
    // Start is called before the first frame update
    public string robotStudioIP = "127.0.0.1";
    public int robotStudioTcpPort = 11000;
    public int robotStudioUdpPort = 10000;

    private UdpClient udpClient;
    private bool ActiveUdp;

    public Transform Articulacion1;
    public Transform Articulacion2;
    public Transform Articulacion3;
    public Transform Articulacion4;
    public Transform Articulacion5;

   

    public void IniciarConexion()
    {
        Debug.Log("Iniciando conexion.");
        // Creamos una conexion con el servidor indicado en la diteccion ip y puerto
        TcpClient tcpclient = new TcpClient(robotStudioIP, robotStudioTcpPort);
        // Obtenemos el networkstream por el que enviaremos los datos
        NetworkStream tcpStream = tcpclient.GetStream();

        try
        {
            // Creamos un mensaje que convertimos a bytes mediante el codigo UTF8
            // y guardamos en la variable data, la cual enviamos por el stream.
            string message = "Hello, server";
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpStream.Write(data, 0, data.Length);

            // La informacion que recibo de robotstudio por el canal de stream lo almaceno
            // en buffer y lo convierto a una cadena de caracterres con UTF8, almacenada en response
            byte[] buffer = new byte[1024];
            int bytesRead = tcpStream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            if (response == "Hello")
            {
                // Si la respuesta del servidor robotStudio es correcta inicion una conexion UDP
                // En un hilo secundario distinto al Tcp, por donde enviare las coordenadas del robot.
                Debug.Log("Conexion correcta");
                ActiveUdp = true;

                Thread udpClient = new Thread(UDPThread);
                udpClient.Start();
            }
            else
            {
                Debug.LogError("Error de mensaje");

                ActiveUdp = false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error de conexion: " + e.Message);
            ActiveUdp = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void udpSend(string udpmessage)
    {
        if (udpClient != null)
        {
            byte[] udpdata = Encoding.UTF8.GetBytes(udpmessage);
            udpClient.Send(udpdata, udpdata.Length, robotStudioIP, robotStudioUdpPort);
        }
    }

    private void UDPThread()
    {
        try
        {
            // Crear el cliente UDP
            udpClient = new UdpClient();

            // Dirección IP y puerto del servidor
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(robotStudioIP), robotStudioUdpPort);

            // Bucle infinito para el envío continuo de datos UDP
            while (ActiveUdp)
            {
                // Obtener los ángulos de rotación de cada articulación
                Vector3 Mov1 = Articulacion1.rotation.eulerAngles;
                Vector3 Mov2 = Articulacion2.rotation.eulerAngles;
                Vector3 Mov3 = Articulacion3.rotation.eulerAngles;
                Vector3 Mov4 = Articulacion4.rotation.eulerAngles;
                Vector3 Mov5 = Articulacion5.rotation.eulerAngles;

                // Extraer los componentes de ángulo relevantes
                float J1 = Mov1.y;
                float J2 = Mov2.z;
                float J3 = Mov3.z;
                float J4 = Mov4.x;
                float J5 = Mov5.z;

                // Crear el mensaje UDP con los ángulos y enviarlo al servidor
                string udpMessage = string.Format("{0},{1},{2},{3},{4}", J1, J2, J3, J4, J5);

                byte[] udpData = Encoding.UTF8.GetBytes(udpMessage);
                udpClient.Send(udpData, udpData.Length, serverEndPoint);

                //UnityMainThreadDispatcher.Enqueue(() => udpSend(udpMessage));

                // Esperar un tiempo antes de enviar el próximo mensaje (ajusta según tus necesidades)
                Thread.Sleep(180);  // Por ejemplo, esperar 100 milisegundos entre envíos
                //yield return new WaitForSeconds(0.1f);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error en el hilo UDP: " + e.Message);
        }
        finally
        {
            // Cerrar el cliente UDP al salir del bucle
            if (udpClient != null)
            {
                udpClient.Close();
                udpClient = null;
                ActiveUdp = false;
            }
        }
    }

    // Destruye la conexion cuando se cierra la aplicacion
    void OnDestroy()
    {
        if (udpClient != null)
        {
            ActiveUdp = false;
            udpClient.Close();
            udpClient = null;
            
        }
    }
}
