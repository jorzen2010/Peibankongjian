using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Peibankongjian.Startup))]
namespace Peibankongjian
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
