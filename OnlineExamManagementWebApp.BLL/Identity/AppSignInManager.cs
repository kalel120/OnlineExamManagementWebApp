using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OnlineExamManagementWebApp.Models.Identity;


namespace OnlineExamManagementWebApp.BLL.Identity {

    public class AppSignInManager : SignInManager<AppUser, int> {

        public AppSignInManager(AppUserManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager) { }
    }
}
