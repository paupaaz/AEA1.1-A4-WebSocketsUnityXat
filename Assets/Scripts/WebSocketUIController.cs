
using UnityEngine;
using UnityEngine.UIElements;

public class WebSocketUIController : MonoBehaviour
{
    [SerializeField]
    private WebSocketClient websocketClient;

    private UIDocument documento;
    private TextField campoMensaje;
    private Button botonEnviar;
    private Label resultado;
    private ScrollView historialChat;

    private void OnEnable()
    {
        documento = GetComponent<UIDocument>();

        campoMensaje = documento.rootVisualElement
            .Q<TextField>("campoMensaje");

        botonEnviar = documento.rootVisualElement
            .Q<Button>("botonEnviar");

        resultado = documento.rootVisualElement
            .Q<Label>("resultado");

        historialChat = documento.rootVisualElement
            .Q<ScrollView>("historialChat");

        if (botonEnviar != null)
        {
            botonEnviar.clicked += EnviarMensaje;
        }
    }

    private void OnDisable()
    {
        if (botonEnviar != null)
        {
            botonEnviar.clicked -= EnviarMensaje;
        }
    }

    private void EnviarMensaje()
    {
        if (campoMensaje == null || historialChat == null)
        {
            Debug.LogError(
                "Falta campoMensaje o historialChat en la interfaz."
            );
            return;
        }

        string mensaje = campoMensaje.value;

        if (string.IsNullOrWhiteSpace(mensaje))
        {
            resultado.text = "Escribe un mensaje primero.";
            return;
        }

        if (websocketClient != null)
        {
            websocketClient.SendWebSocketMessage(mensaje);

            // Creamos una nueva línea para el mensaje
            Label nuevoMensaje = new Label("Tú: " + mensaje);

            // Añadimos el mensaje al historial
            historialChat.Add(nuevoMensaje);

            // Bajamos automáticamente al último mensaje
            historialChat.ScrollTo(nuevoMensaje);

            // Limpiamos el campo de escritura
            campoMensaje.value = "";

            if (resultado != null)
            {
                resultado.text = "";
            }
        }
        else
        {
            if (resultado != null)
            {
                resultado.text =
                    "Asigna el WebSocketClient en el Inspector.";
            }

            Debug.LogError("Falta asignar el WebSocketClient.");
        }
    }
}
