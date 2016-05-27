using HSDraft.Logic;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HSDraft.Startup))]
namespace HSDraft
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            SessionData.DraftContainer = new System.Collections.Generic.List<Models.Draft.Draft>();
            ConfigureAuth(app);

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}
