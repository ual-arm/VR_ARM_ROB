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

public class Comunicacion_irb120 : MonoBehaviour
{
    // Start is called before the first frame update
    public string robotStudioIP = "127.0.0.1";
    public int robotStudioTcpPort = 10000;
    public TcpClient tcpClient;
    NetworkStream tcpStream;


    private bool ActiveC;
    private bool loopsend;

    public Transform Articulacion1;
    public Transform Articulacion2;
    public Transform Articulacion3;
    public Transform Articulacion4;
    public Transform Articulacion5;



    public void IniciarConexion()
    {
        Debug.Log("Iniciando conexion.");
        // Creamos una conexion con el servidor indicado en la diteccion ip y puerto

        //TcpClient tcpClient = new TcpClient(robotStudioIP, robotStudioTcpPort);
        //NetworkStream tcpStream = tcpclient.GetStream();

        // He probado a definir las variables arriba.
        tcpClient = new TcpClient(robotStudioIP, robotStudioTcpPort);
        tcpStream = tcpClient.GetStream();

        // Gpt me da tra posible solucion: Parte1
            //tcpClient = new TcpClient();



        try
        {

            // Gpt me da tra posible solucion: Parte1
                //tcpClient.Connect(robotStudioIP, robotStudioTcpPort);
                //tcpStream = tcpClient.GetStream();

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
                ActiveC = true;
                loopsend = true;

                StartCoroutine(EjecutarCadaSegundo());

            }
            else
            {
                Debug.LogError("Error de conexion");

                ActiveC = false;
                //tcpclient.Close();
                //tcpclient = null;
                //tcpStream.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error de conexion: " + e.Message);
            ActiveC = false;
            tcpClient.Close();
            tcpClient = null;
            tcpStream.Close();
        }
    }

    IEnumerator EjecutarCadaSegundo()
    {
        while (loopsend==true)
        {

            Vector3 Mov1 = Articulacion1.rotation.eulerAngles;
            Vector3 Mov2 = Articulacion2.rotation.eulerAngles;
            Vector3 Mov3 = Articulacion3.rotation.eulerAngles;
            Vector3 Mov4 = Articulacion4.rotation.eulerAngles;
            Vector3 Mov5 = Articulacion5.rotation.eulerAngles;

            // Extraer los componentes de ángulo relevantes
            //float J1 = Mov1.y;
            //float J2 = Mov2.z;
            //float J3 = Mov3.z;
            //float J4 = Mov4.x;
            //float J5 = Mov5.z;

            float J1 = Articulacion1.localEulerAngles.y;
            float J2 = Articulacion2.localEulerAngles.z;
            float J3 = Articulacion3.localEulerAngles.z;
            float J4 = Articulacion4.localEulerAngles.x;
            float J5 = Articulacion5.localEulerAngles.z;

            // Crear el mensaje UDP con los ángulos y enviarlo al servidor
            string CoordenateMessage = string.Format("{0};{1};{2};{3};{4}", J1, J2, J3, J4, J5);
            byte[] CoordenateData = Encoding.UTF8.GetBytes(CoordenateMessage);
            tcpStream.Write(CoordenateData, 0, CoordenateData.Length);

            byte[] buffercontrol = new byte[1024];
            int bytesRecived = tcpStream.Read(buffercontrol, 0, buffercontrol.Length);
            string control = Encoding.UTF8.GetString(buffercontrol, 0, bytesRecived);

            if (control == "ok")
            {
                loopsend = true;
            }
            else
            {
                 loopsend = false;
            }

            yield return new WaitForSeconds(0.2f);

            // Realiza la acción que deseas ejecutar cada segundo
        }
    }

    // Destruye la conexion cuando se cierra la aplicacion
    void OnDestroy()
    {
        
            ActiveC = false;
            tcpClient.Close();
            tcpStream.Close();
      
    }
}
