using Microsoft.Web.WebSockets;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DemoWebSocket.Controllers
{
    /// <summary>
    /// Exemplo básico de chat Websocket sem o emprego de SignalR
    /// </summary>
    public class TesteSocketsController : ApiController
    {
        public HttpResponseMessage Get(string username)
        {
            HttpContext.Current.AcceptWebSocketRequest(new ChatWebSocketHandler(username));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        class ChatWebSocketHandler : WebSocketHandler
        {
            private static WebSocketCollection _chatClients = new WebSocketCollection();
            private string _username;

            public ChatWebSocketHandler(string username)
            {
                _username = username;
            }

            public override void OnOpen()
            {
                _chatClients.Broadcast(_username + " entrou");
                _chatClients.Add(this);
            }

            public override void OnClose()
            {
                _chatClients.Remove(this);
                _chatClients.Broadcast(_username + " saiu");
                Send("Você saiu");
            }

            public override void OnMessage(string message)
            {
                _chatClients.Broadcast(_username + ": " + message);
            }
        }
    }
}