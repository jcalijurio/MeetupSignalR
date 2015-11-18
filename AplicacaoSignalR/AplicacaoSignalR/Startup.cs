using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(ChatBasicoSignalR.Startup))]
namespace ChatBasicoSignalR
{
    /// <summary>
    /// Arquivo de configuração de aplicações .net baseadas nos padrões OWIN
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Exemplo de configuração permitindo acesso externo através de JSONP/Cors
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    EnableJSONP = true
                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}