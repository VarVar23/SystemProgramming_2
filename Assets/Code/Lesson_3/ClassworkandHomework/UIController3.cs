using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController3 : MonoBehaviour
{
    [SerializeField] private Button _startServerButton;
    [SerializeField] private Button _shutDownServerButton;
    [SerializeField] private Button _connectClientButton;
    [SerializeField] private Button _disconectClientButton;
    [SerializeField] private Button _sendMessageButton;

    [SerializeField] TMP_InputField _inputField;
    [SerializeField] TextField3 _textField;
    [SerializeField] private Server3 _server;
    [SerializeField] private Client3 _client;

    void Start()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        _startServerButton.onClick.AddListener(StartServer);
        _shutDownServerButton.onClick.AddListener(ShutDownServer);
        _connectClientButton.onClick.AddListener(Connect);
        _disconectClientButton.onClick.AddListener(Disconnect);
        _sendMessageButton.onClick.AddListener(SendMessage);
        _client.OnMessageReceiveEvent += ReceiveMessage;
    }

    private void StartServer()
    {
        _startServerButton.enabled = false;
        _connectClientButton.enabled = false;
        _disconectClientButton.enabled = false;
        _sendMessageButton.enabled = false;
        _server.StartServer();
        _textField.StartMessage("Йоу, теперь ты сервер!");
    }

    private void ShutDownServer()
    {
        _server.ShutDownServer();
    }

    private void Connect()
    {
        _client.Connect();
        _textField.StartMessage("Пожалуйста, введите свое имя в строке ввода сообщения");
    }

    private void Disconnect()
    {
        _client.Disconnect();
    }

    private void SendMessage()
    {
        _client.SendMessage(_inputField.text);
        _inputField.text = "";
    }

    private void ReceiveMessage(object message)
    {
        _textField.ReceiveMessage(message);
    }

    private void Update()
    {
        if (Input.GetKeyDown("enter"))
        {
            SendMessage();
        }
    }
}


