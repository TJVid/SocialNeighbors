using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialNeighbors.Startup))]
namespace SocialNeighbors
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
