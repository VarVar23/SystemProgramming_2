using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Server3 : MonoBehaviour
{
    private const int _maxConnection = 10;
    private int _port = 5888;
    private int _hostID;
    private int _reliableChannel;
    private bool _isStarted = false;
    private byte _error;
    private bool _noName;
    private Dictionary<int, string> _connectionID = new Dictionary<int, string>();

    private void Update()
    {
        if (!_isStarted) return;

        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;

        NetworkEventType recData = NetworkTransport.Receive(out connectionId, out connectionId, out channelId,
            recBuffer, bufferSize, out dataSize, out _error);

        while (recData != NetworkEventType.Nothing)
        {
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;

                case NetworkEventType.DisconnectEvent:
                    _connectionID.Remove(connectionId);

                    SendMessageAll("Пользователь " + _connectionID[connectionId] + " покинул нас :(");
                    break;

                case NetworkEventType.DataEvent:
                    string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);

                    if (_noName)
                    {
                        _connectionID.Add(connectionId, message);
                        _noName = false;
                        SendMessageAll("У нас новый пользователь! " + _connectionID[connectionId] + ", привет!");
                    }
                    else
                    {
                        SendMessageAll(_connectionID[connectionId] + ": " + message);
                    }
                    break;

                case NetworkEventType.ConnectEvent:
                    _noName = true;
                    break;

                case NetworkEventType.BroadcastEvent:
                    break;
            }

            recData = NetworkTransport.Receive(out _hostID, out connectionId, out connectionId,
                recBuffer, bufferSize, out dataSize, out _error);
        }
    }

    public void StartServer()
    {
        NetworkTransport.Init();

        ConnectionConfig connectionConfig = new ConnectionConfig();
        _reliableChannel = connectionConfig.AddChannel(QosType.Reliable);
        HostTopology topology = new HostTopology(connectionConfig, _maxConnection);
        _hostID = NetworkTransport.AddHost(topology, _port);

        _isStarted = true;
    }

    public void ShutDownServer()
    {
        if (!_isStarted) return;

        NetworkTransport.RemoveHost(_hostID);
        NetworkTransport.Shutdown();
        _isStarted = false;
    }

    public void SendMessage(string message, int connectionID)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        int size = message.Length * sizeof(char);
        NetworkTransport.Send(_hostID, connectionID, _reliableChannel, buffer, size, out _error);

        if ((NetworkError)_error != NetworkError.Ok)
        {
            Debug.LogError((NetworkError)_error);
        }
    }

    public void SendMessageAll(string message)
    {
        foreach (var connection in _connectionID.Keys)
        {
            SendMessage(message, connection);
        }
    }
}