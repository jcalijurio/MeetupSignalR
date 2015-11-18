using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TesteServerEvents.Startup))]
namespace TesteServerEvents
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
