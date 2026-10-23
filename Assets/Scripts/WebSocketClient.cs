
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NativeWebSocket;

public class WebSocketClient : MonoBehaviour
{
    [Header("Configuració del WebSocket")]
    [SerializeField]
    private string websocketUrl = "ws://localhost:3030";

    private WebSocket websocket;

    async void Start()
    {
        await ConnectWebSocket();
    }

    private async Task ConnectWebSocket()
    {
        websocket = new WebSocket(websocketUrl);

        websocket.OnOpen += () =>
        {
            Debug.Log("WebSocket connectat!");
        };

        websocket.OnError += (error) =>
        {
            Debug.LogError("WebSocket error: " + error);
        };

        websocket.OnClose += (code) =>
        {
            Debug.Log("WebSocket tancat!");
        };

        websocket.OnMessage += (bytes) =>
        {
            string mensaje = Encoding.UTF8.GetString(bytes);
            Debug.Log("Missatge rebut: " + mensaje);
        };

        await websocket.Connect();
    }

    public async void SendWebSocketMessage(string mensaje)
    {
        Debug.Log("Enviant missatge: " + mensaje);
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            await websocket.SendText(mensaje);
            Debug.Log("Missatge enviat: " + mensaje);
        }
        else
        {
            Debug.LogWarning("La connexió WebSocket no està oberta.");
        }
    }

    void Update()
    {
        if (websocket != null)
        {
            websocket.DispatchMessageQueue();
        }
    }

    private async void OnApplicationQuit()
    {
        if (websocket != null)
        {
            await websocket.Close();
        }
    }
}
