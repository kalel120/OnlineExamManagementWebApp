using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using OnlineExamManagementWebApp.BLL.Identity;
using Microsoft.AspNet.Identity.Owin;
using OnlineExamManagementWebApp.Models.Identity;
using OnlineExamManagementWebApp.ViewModels;

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

        [HttpPost]
        public ActionResult Register(RegisterViewModel viewModel) {
            if (ModelState.IsValid) {
                var user = Mapper.Map<AppUser>(viewModel);
                var result = UserManager.Create(user, viewModel.Password);

                if (result.Succeeded) {
                    return RedirectToAction("List", "Course", null);
                }
            }
            return View();
        }
    }
}