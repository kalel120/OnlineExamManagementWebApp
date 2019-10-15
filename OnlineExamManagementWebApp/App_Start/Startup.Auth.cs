using Owin;
using OnlineExamManagementWebApp.Config;

namespace OnlineExamManagementWebApp {
    public partial class Startup {

        public void ConfigureAuth(IAppBuilder app) {
            AppConfiguration.AuthConfiguration(app);
        }
    }
}