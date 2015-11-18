using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DemoWebSocket
{
    public class TesteSignalRHub : Hub
    {
        public override System.Threading.Tasks.Task OnConnected()
        {
            var nome = Context.QueryString["nome"];
            Clients.Others.MensagemGeral(nome + " entrou");
            return base.OnConnected();
        }

        public void Enviar(string mensagem)
        {
            var nome = Context.QueryString["nome"];
            Clients.All.MensagemGeral(nome + ": " + mensagem);
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var nome = Context.QueryString["nome"];
            Clients.All.MensagemGeral(nome + " saiu");
            return base.OnDisconnected(stopCalled);
        }
    }
}