using Microsoft.AspNet.Identity;
using OnlineExamManagementWebApp.Models.Identity;

namespace OnlineExamManagementWebApp.BLL.Identity {
    public class AppUserManager : UserManager<AppUser, int> {

        public AppUserManager(IUserStore<AppUser, int> store) : base(store) {
        }
    }
}
