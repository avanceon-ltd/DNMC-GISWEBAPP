using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppForm.Startup))]
namespace WebAppForm
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
