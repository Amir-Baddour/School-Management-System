using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Testing1.Startup))]
namespace Testing1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
