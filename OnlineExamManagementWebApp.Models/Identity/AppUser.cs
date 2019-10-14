using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineExamManagementWebApp.Models.Identity {
    public class AppUser : IdentityUser<int, IdentityUserLogin<int>, AppUserRole, IdentityUserClaim<int>> {

        public AppUser() {

        }
    }
}
