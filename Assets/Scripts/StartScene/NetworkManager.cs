using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviour
{
    //components
    public TcpClient tcpClient;
    public StreamReader streamReader;
    public StreamWriter streamWriter;
    public NetworkStream networkStream;
    public bool socketReady;
    
    //msg
    public string errorMessage ;
    
    //data buffer
    private char[] buffer = new char[1024];
    private string _data;
    public bool finishedReceiving;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    
    private void Update()
    {
        ProcessReceive();
    }

    public void StartConnection(string host, int port)
    {
        SetUpSocket(host, port);
    }

    public void Send(string data)
    {
        streamWriter.Write(data);
        streamWriter.Flush();
    }
    
    public void Receive()
    {
        //StartCoroutine(ProcessReceive());
        finishedReceiving = false;
    }

    public string FetchReceivedData()
    {
        var result = string.Copy(_data);
        _data = "";
        buffer = new char[buffer.Length];
        return result;
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void SetUpSocket(string host, int port)
    {
        try
        {
            tcpClient = new TcpClient(host, port);
            networkStream = tcpClient.GetStream();
            tcpClient.NoDelay = true;
            streamWriter = new StreamWriter(networkStream);
            streamReader = new StreamReader(networkStream);
            socketReady = true;
        }
        catch (Exception e)
        {
            errorMessage = e.Message;
        }
    }

    private  void CloseSocket()
    {
        if (!socketReady) return;
        streamReader.Close();
        streamWriter.Close();
        networkStream.Close();
        tcpClient.Close();
        socketReady = false;
        Debug.Log("Client Socket Closed");
    }


    private void ProcessReceive()
    {
        //yield return new WaitUntil(()=>networkStream.DataAvailable);
        if (!socketReady) return;
        if (finishedReceiving) return;
        if (!networkStream.DataAvailable) return;
        
        var bytes = streamReader.Read(buffer, 0, buffer.Length);
        _data += new string(buffer,0,bytes);
        
        //end receiving
        if(bytes < buffer.Length)
        {
            finishedReceiving = true;
            Debug.Log("Finished receiving");
            Debug.Log("Received data:" + _data);
        }
    }
    
}
