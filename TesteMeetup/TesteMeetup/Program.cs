using Microsoft.AspNet.SignalR.Client;
using System;

namespace DemoDesktopConsole
{
    /// <summary>
    /// Exemplo de cliente .Net para acesso ao SignalR
    /// </summary>
    class Program
    {
        // URL disponível Azure Websites
        private static readonly string _url = "http://meetupsignalrxamarin.azurewebsites.net";
        static void Main(string[] args)
        {
            // conexão ao servidor e instanciamento do proxy de conexão ao hub ChatHub.
            var connection = new HubConnection(_url);
            var proxy = connection.CreateHubProxy("ChatHub");

            // Método de retorno ao receber mensagem do servidor.
            proxy.On<string, string>("broadcastMessage", (name, message) =>
            {
                Console.WriteLine("Retornou: " + name + " => " + message);
            });

            try
            {
                // Iniciando a conexão com o servidor podendo definir o tipo de transporte preferencial.
                //connection.Start(new LongPollingTransport());
                connection.Start();
            }
            catch
            {
                Console.WriteLine("Deu erro");
                return;
            }

            Console.WriteLine("Conectado");
            Console.WriteLine(connection.Transport.Name);

            while (true)
            {
                // Chamada ao servidor ao pressionar enter.
                proxy.Invoke("Send", "Console", Console.ReadLine());
            }
        }
    }
}
