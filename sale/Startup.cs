using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sale.Startup))]
namespace sale
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
