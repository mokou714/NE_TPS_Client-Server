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
    //singleton

    //components
    private TcpClient tcpClient;
    private StreamReader streamReader;
    private StreamWriter streamWriter;
    private NetworkStream networkStream;
    private bool socketReady;
    
    //msg
    public string errorMessage ;
    
    //data buffer
    private char[] buffer = new char[1024];
    private string _data;
    public bool finishedReceiving;

    public static NetworkManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance is null)
        {
            Debug.Log("Created NetworkManager instance");
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(this);
            Debug.Log("Destroy extra NetworkManager instance");
        }
        
    }
    
    
    private void Update()
    {
        ProcessReceive();
    }

    public bool StartConnection(string host, int port)
    {
        return SetUpSocket(host, port);
    }

    public void Send(string data)
    {
        if (!socketReady) return;
        
        streamWriter.Write(data);
        streamWriter.Flush();
    }
    
    public void Receive()
    {
        if (!socketReady) return;
        
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

    public bool IsReady()
    {
        return socketReady;
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private bool SetUpSocket(string host, int port)
    {
        try
        {
            tcpClient = new TcpClient(host, port);
            networkStream = tcpClient.GetStream();
            tcpClient.NoDelay = true;
            streamWriter = new StreamWriter(networkStream);
            streamReader = new StreamReader(networkStream);
            socketReady = true;
            return true;
        }
        catch (Exception e)
        {
            errorMessage = e.Message;
            return false;
        }
    }

    private void CloseSocket()
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
