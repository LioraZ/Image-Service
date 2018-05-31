using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImageWeb.Startup))]
namespace ImageWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
