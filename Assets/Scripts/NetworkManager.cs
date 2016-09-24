using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;

public class NetworkManager : MonoBehaviour {

    static internal Boolean socketReady = false;
    static TcpClient mySocket;
    NetworkStream theStream;
    static StreamWriter theWriter;
    static StreamReader theReader;
    String Host = "217.150.77.46";
    Int32 Port = 6969;

    void Start()
    {
        setupSocket();
    }

    void FixedUpdate()
    {
        if (!readSocket().Equals(""))
            ServerResponseHandler.HandleServerResponse(readSocket());
    }

    public void setupSocket()
    {
        try
        {
            mySocket = new TcpClient(Host, Port);
            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);
            theReader = new StreamReader(theStream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
        }
    }

    public static void writeSocket(string theLine)
    {
        if (!socketReady)
            return;
        String foo = theLine + "\r\n";
        theWriter.Write(foo);
        theWriter.Flush();
    }

    public String readSocket()
    {
        if (!socketReady)
            return "";
        if (theStream.DataAvailable)
            return theReader.ReadLine();
        return "";
    }

    public void closeSocket()
    {
        if (!socketReady)
            return;
        theWriter.Close();
        theReader.Close();
        mySocket.Close();
        socketReady = false;
    }


}
