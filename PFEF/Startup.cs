using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PFEF.Startup))]
namespace PFEF
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
