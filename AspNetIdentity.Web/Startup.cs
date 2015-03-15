using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AspNetIdentity.Web.Startup))]
namespace AspNetIdentity.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
