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
//using UnityEditor.PackageManager;
using UnityEngine.SocialPlatforms;

public class Comunicacionprueba1 : MonoBehaviour
{
    // Start is called before the first frame update
    public string robotStudioIP = "127.0.0.1";
    public int robotStudioTcpPort = 11000;
    public int robotStudioUdpPort = 10000;

    private TcpClient udpClient;
    private bool ActiveUdp;
    private bool aux;

    public Transform Articulacion1;
    public Transform Articulacion2;
    public Transform Articulacion3;
    public Transform Articulacion4;
    public Transform Articulacion5;

    private Thread udpThread;
    private bool activeUdp1;
    private bool aux1;
    NetworkStream udpStream;

    float J1, J2, J3, J4, J5;
    float localj1, localj2, localj3, localj4, localj5;
    private readonly object lockObject = new object();

    public void IniciaConexion()
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
                aux = true;

                udpThread = new Thread(() =>UDPThread(udpStream));
                udpThread.Start();

                //UDPConect(ActiveUdp, aux);

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

    public void IniciarConexion()
    {
        udpStream=Iniciar();
    }
    private NetworkStream Iniciar()
    {
        TcpClient udpClient = new TcpClient(robotStudioIP, robotStudioUdpPort);
        NetworkStream udpStream = udpClient.GetStream();
        return udpStream;
    }

    // Update is called once per frame
    void Update()
    {
        // Obtener los ángulos de rotación de cada articulación en el hilo principal
        Vector3 Mov1 = Articulacion1.rotation.eulerAngles;
        Vector3 Mov2 = Articulacion2.rotation.eulerAngles;
        Vector3 Mov3 = Articulacion3.rotation.eulerAngles;
        Vector3 Mov4 = Articulacion4.rotation.eulerAngles;
        Vector3 Mov5 = Articulacion5.rotation.eulerAngles;

        // Extraer los componentes de ángulo relevantes
        
            J1 = Mov1.y;
            J2 = Mov2.z;
            J3 = Mov3.z;
            J4 = Mov4.x;
            J5 = Mov5.z;
        
        string udpMessage = string.Format("{0};{1};{2};{3};{4}", J1, J2, J3, J4, J5);
        udpSend(udpMessage, udpStream);
    }
    void udpSend(string udpmessage, NetworkStream udpStream)
    {

        Debug.Log("He enviado un dato" + udpmessage);
        byte[] udpdata = Encoding.UTF8.GetBytes(udpmessage);
        //udpClient.Send(udpdata, udpdata.Length, robotStudioIP, robotStudioUdpPort);
        // NetworkStream udpStream = udpClient.GetStream();
        udpStream.Write(udpdata, 0, udpdata.Length);
        //}
    }

    private void UDPConect(bool ActiveUdp, bool aux)
    {

        try
        {

            Debug.Log("EStoy en thread");

            TcpClient udpClient = new TcpClient(robotStudioIP, robotStudioUdpPort);
            NetworkStream udpStream = udpClient.GetStream();



            while (ActiveUdp == true && aux == true)
            {

                // Obtener los ángulos de rotación de cada articulación
                Vector3 Mov1 = Articulacion1.rotation.eulerAngles;
                Vector3 Mov2 = Articulacion2.rotation.eulerAngles;
                Vector3 Mov3 = Articulacion3.rotation.eulerAngles;
                Vector3 Mov4 = Articulacion4.rotation.eulerAngles;
                Vector3 Mov5 = Articulacion5.rotation.eulerAngles;

                // Extraer los componentes de ángulo relevantes

                J1 = Mov1.y;
                J2 = Mov2.z;
                J3 = Mov3.z;
                J4 = Mov4.x;
                J5 = Mov5.z;

                Debug.Log("Bucle thread");

                localj1 = J1;
                localj2 = J2;
                localj3 = J3;
                localj4 = J4;
                localj5 = J5;

                //byte[] buf = new byte[1024];
                //int bytesRead = udpStream.Read(buf, 0, buf.Length);
                //string coordinate = Encoding.UTF8.GetString(buf, 0, bytesRead);
                string udpMessage = string.Format("{0};{1};{2};{3};{4}", localj1, localj2, localj3, localj4, localj5);
                udpSend(udpMessage, udpStream);

                bool auxt = false;

                while (auxt == false)
                {
                    byte[] buff = new byte[1024];
                    int bytescontrol = udpStream.Read(buff, 0, buff.Length);
                    string control = Encoding.UTF8.GetString(buff, 0, bytescontrol);

                    if (control == "ok")
                    {
                        Debug.Log("Control correcta");

                        auxt = true;
                    }
                }


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


    private IEnumerator WaitAndContinue()
    {
        // Realizar alguna operación antes de la pausa (si es necesario)

        yield return new WaitForSeconds(1.0f);  // Esperar 1 segundo

        // Realizar alguna operación después de la pausa (si es necesario)

        // ... (otros procesamientos)
    }



    private void UDPThread(NetworkStream udpStream)
    {
        try
        {
            Debug.Log("Estoy en el hilo UDP");

            TcpClient udpClient = new TcpClient(robotStudioIP, robotStudioUdpPort);
            udpStream = udpClient.GetStream();


                // Crear el mensaje UDP con las coordenadas y enviarlo al servidor


                // Esperar un tiempo antes de enviar el próximo mensaje (ajusta según tus necesidades)
                Thread.Sleep(10);
            
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
                //activeUdp = false;
            }
        }
    }


}
