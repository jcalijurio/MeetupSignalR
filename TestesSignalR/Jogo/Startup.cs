using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Jogo.Startup))]

namespace Jogo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
