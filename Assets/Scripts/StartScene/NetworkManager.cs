using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    //components
    public static TcpClient tcpClient;
    public static StreamReader streamReader;
    public static StreamWriter streamWriter;
    public static NetworkStream networkStream;
    public static bool socketReady;
    
    //msg
    public static string errorMessage ;
    

    public static void StartConnection(string host, int port)
    {
        SetUpSocket(host, port);
    }

    public static void Send(string data)
    {
        streamWriter.Write(data);
        streamWriter.Flush();
    }

    public static string Receive()
    {
        return streamReader.ReadToEnd();
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private static void SetUpSocket(string host, int port)
    {
        try
        {
            tcpClient = new TcpClient(host, port);
            networkStream = tcpClient.GetStream();
            streamWriter = new StreamWriter(networkStream);
            streamReader = new StreamReader(networkStream);
            socketReady = true;
            
            Send("t;e;s;t");
        }
        catch (Exception e)
        {
            errorMessage = e.Message;
        }
    }

    private static void CloseSocket()
    {
        if (!socketReady) return;
        streamReader.Close();
        streamWriter.Close();
        networkStream.Close();
        tcpClient.Close();
        socketReady = false;
    }
}
