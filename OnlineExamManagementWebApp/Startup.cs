using Owin;
using Microsoft.Owin;

[assembly: OwinStartupAttribute(typeof(OnlineExamManagementWebApp.Startup))]

namespace OnlineExamManagementWebApp {
    public partial class Startup {

        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }

    }
}
