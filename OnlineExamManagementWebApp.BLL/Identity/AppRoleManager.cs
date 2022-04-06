using Microsoft.AspNet.Identity;
using OnlineExamManagementWebApp.Models.Identity;

namespace OnlineExamManagementWebApp.BLL.Identity {
    public class AppRoleManager : RoleManager<AppRole, int> {

        public AppRoleManager(IRoleStore<AppRole, int> store) : base(store) {

        }
    }
}
