using Microsoft.AspNet.SignalR;

namespace ChatBasicoSignalR
{
    /// <summary>
    /// Hub de acesso ao Chat
    /// </summary>
    public class ChatHub : Hub
    {
        /// <summary>
        /// Recebe a chamada "Send"
        /// </summary>
        /// <param name="name">nome da plataforma/usuário</param>
        /// <param name="message">mensagem enviada</param>
        public void Send(string name, string message)
        {
            // Chamada ao método broadcastMessage do cliente.
            Clients.All.broadcastMessage(name, message);
        }
    }
}