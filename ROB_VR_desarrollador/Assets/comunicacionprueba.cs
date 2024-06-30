using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Comunicacionprueba : MonoBehaviour
{
    public string robotStudioIP = "127.0.0.1";
    public int robotStudioTcpPort = 11000;
    public int robotStudioUdpPort = 10000;

    private TcpClient udpClient;
    private NetworkStream udpStream;
    private bool ActiveUdp;
    private bool aux;
    private object lockObject = new object();

    public Transform Articulacion1;
    public Transform Articulacion2;
    public Transform Articulacion3;
    public Transform Articulacion4;
    public Transform Articulacion5;

    float J1, J2, J3, J4, J5;



    public void IniciarConexion()
    {
        Debug.Log("Iniciando conexión.");
        TcpClient udpClient = new TcpClient(robotStudioIP, robotStudioUdpPort);
        udpStream = udpClient.GetStream();

    }
    void Update()
    {
        Debug.Log("up");
        // Obtener los ángulos de rotación de cada articulación en el hilo principal
        Vector3 Mov1 = Articulacion1.rotation.eulerAngles;
        Vector3 Mov2 = Articulacion2.rotation.eulerAngles;
        Vector3 Mov3 = Articulacion3.rotation.eulerAngles;
        Vector3 Mov4 = Articulacion4.rotation.eulerAngles;
        Vector3 Mov5 = Articulacion5.rotation.eulerAngles;

        // Extraer los componentes de ángulo relevantes
        lock (lockObject)
        {
            J1 = Mov1.y;
            J2 = Mov2.z;
            J3 = Mov3.z;
            J4 = Mov4.x;
            J5 = Mov5.z;
        }

        string udpMessage = string.Format("{0};{1};{2};{3};{4}", J1, J2, J3, J4, J5);
        udpSend(udpMessage, udpStream);
    }

    void OnDestroy()
    {
        // Cerrar el cliente UDP al salir de la aplicación
        if (udpClient != null)
        {
            ActiveUdp = false;
            udpClient.Close();
            udpClient = null;
        }
    }

    void udpSend(string udpmessage, NetworkStream udpStream)
    {
        Debug.Log("He enviado un dato" + udpmessage);
        byte[] udpdata = Encoding.UTF8.GetBytes(udpmessage);
        udpStream.Write(udpdata, 0, udpdata.Length);
    }

    
}
