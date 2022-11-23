using UnityEngine;
using UnityEngine.Networking;

public class Classwork_LLAPI : MonoBehaviour
{
    private const int _countConnections = 10;
    private const int _port = 8888;
    private byte _error;
    private int _bufferSize = 1024;
    private byte[] _buffer = new byte[1024];
    private int _dataSize = 500;
    private int _hostID;
    private int _connectionID;
    private int _TCPchannel;
    private void Start()
    {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        _TCPchannel = config.AddChannel(QosType.Reliable); //TCP - Reliable, UDP - Unreliable //Обязательно дойдет и не обязательно
        HostTopology hostTopology = new HostTopology(config, _countConnections);

        _hostID = NetworkTransport.AddHost(hostTopology, _port);
        _connectionID = NetworkTransport.Connect(_hostID, "127.0.0.1", _port, 0, out _error);

        NetworkTransport.Send(_hostID, _connectionID, _TCPchannel, _buffer, _bufferSize, out _error); // Отправить
        NetworkTransport.Disconnect(_hostID, _connectionID, out _error); // Отключиться

        if((NetworkError)_error != NetworkError.Ok)
        {
            Debug.Log((NetworkError)_error);
        }

        NetworkTransport.Receive(out _hostID, out _connectionID, out _TCPchannel, _buffer, _bufferSize, out _dataSize, out _error); // Получить данные
        NetworkTransport.ReceiveFromHost(_hostID, out _connectionID, out _TCPchannel, _buffer, _bufferSize, out _dataSize, out _error); // Получить данные от конкретного хоста
    }

    private void Update()
    {
        NetworkEventType recData = NetworkTransport.Receive(out _hostID, out _connectionID, out _TCPchannel, _buffer, _bufferSize, out _dataSize, out _error);
        
        switch(recData)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.DisconnectEvent:
                break;
            case NetworkEventType.DataEvent:
                break;
            case NetworkEventType.ConnectEvent:
                break;
            case NetworkEventType.BroadcastEvent:
                break;
        }
    }
}
