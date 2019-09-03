using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrabajoFinalWeb.Startup))]
namespace TrabajoFinalWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
