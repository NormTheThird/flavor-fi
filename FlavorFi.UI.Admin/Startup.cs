using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FlavorFi.UI.Admin.Startup))]
namespace FlavorFi.UI.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
