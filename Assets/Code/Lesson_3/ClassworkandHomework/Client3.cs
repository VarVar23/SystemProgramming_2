using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Client3 : MonoBehaviour
{
    public delegate void OnMessageReceive(object message);
    public event OnMessageReceive OnMessageReceiveEvent;

    private const int _maxConnection = 10;
    private int _port = 0;
    private int _serverPort = 5888;
    private int _hostID;
    private int _reliableChannel;
    private int _connectionID;

    private bool _isConnected = false;
    private byte _error;

    public void Connect()
    {
        NetworkTransport.Init();
        ConnectionConfig connectionConfig = new ConnectionConfig();
        _reliableChannel = connectionConfig.AddChannel(QosType.Reliable);
        HostTopology hostTopology = new HostTopology(connectionConfig, _maxConnection);
        _hostID = NetworkTransport.AddHost(hostTopology, _port);
        _connectionID = NetworkTransport.Connect(_hostID, "192.168.0.188", _serverPort, 0, out _error); 

        if ((NetworkError)_error == NetworkError.Ok) 
        {
            _isConnected = true;
        }
        else
        {
            Debug.LogError((NetworkError)_error);
        }
    }

    public void Disconnect()
    {
        if (!_isConnected) return;

        NetworkTransport.Disconnect(_hostID, _connectionID, out _error);
        _isConnected = false;
    }

    private void Update()
    {
        if (!_isConnected) return;

        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;

        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId,
            recBuffer, bufferSize, out dataSize, out _error);

        while (recData != NetworkEventType.Nothing)
        {
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;

                case NetworkEventType.DisconnectEvent:
                    _isConnected = false;
                    OnMessageReceiveEvent?.Invoke("Вы были отсоеденины");
                    break;

                case NetworkEventType.DataEvent:
                    string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    OnMessageReceiveEvent?.Invoke(message);
                    break;

                case NetworkEventType.ConnectEvent:
                    break;

                case NetworkEventType.BroadcastEvent:
                    break;
            }

            recData = NetworkTransport.Receive(out _hostID, out connectionId, out connectionId,
                recBuffer, bufferSize, out dataSize, out _error);
        }
    }

    public void SendMessage(string message)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        int size = message.Length * sizeof(char);
        NetworkTransport.Send(_hostID, _connectionID, _reliableChannel, buffer, size, out _error);

        if ((NetworkError)_error != NetworkError.Ok)
        {
            Debug.LogError((NetworkError)_error);
        }
    }
}