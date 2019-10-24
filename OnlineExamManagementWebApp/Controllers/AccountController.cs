using System.Web;
using System.Web.Mvc;
using OnlineExamManagementWebApp.BLL.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace OnlineExamManagementWebApp.Controllers {
    public class AccountController : Controller {

        public AppUserManager UserManager {
            get {
                return HttpContext.GetOwinContext().Get<AppUserManager>();
            }
        }

        public AppRoleManager RoleManager {
            get {
                return HttpContext.GetOwinContext().Get<AppRoleManager>();

            }
        }

        public AppSignInManager SignInManager {
            get {
                return HttpContext.GetOwinContext().Get<AppSignInManager>();

            }
        }

        /* Registration */
        public ActionResult Register() {
            return View();
        }
    }
}