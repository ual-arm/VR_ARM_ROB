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

public class pruebacomunicacion : MonoBehaviour
{
    public string robotStudioIP = "127.0.0.1";
    public int robotStudioTcpPort = 1234;  // Cambiado a 1234

    private TcpListener tcpListener;

    void Start()
    {
        tcpListener = new TcpListener(IPAddress.Parse(robotStudioIP), robotStudioTcpPort);
        tcpListener.Start();

        // Iniciar un hilo para manejar las conexiones
        Thread listenerThread = new Thread(new ThreadStart(ListenForClients));
        listenerThread.Start();
    }

    private void ListenForClients()
    {
        while (true)
        {
            // Esperar a que se conecte un cliente
            TcpClient client = tcpListener.AcceptTcpClient();

            // Iniciar un hilo para manejar la comunicación con el cliente
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
            clientThread.Start(client);
        }
    }

    private void HandleClientComm(object clientObj)
    {
        TcpClient tcpClient = (TcpClient)clientObj;
        NetworkStream clientStream = tcpClient.GetStream();

        byte[] message = Encoding.UTF8.GetBytes("Hello, client");  // Cambiado a "Hello, client"
        clientStream.Write(message, 0, message.Length);

        tcpClient.Close();
    }
}
