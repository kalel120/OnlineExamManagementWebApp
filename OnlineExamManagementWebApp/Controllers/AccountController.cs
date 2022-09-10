using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using OnlineExamManagementWebApp.BLL.Identity;
using Microsoft.AspNet.Identity.Owin;
using OnlineExamManagementWebApp.Models.Identity;
using OnlineExamManagementWebApp.ViewModels.Account;
using Microsoft.Owin.Security;

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

        #region Registration
        [AllowAnonymous]
        public ActionResult Register() {
            var viewModel = new RegisterViewModel {
                RoleList = GetRoles()
            };
            return View(viewModel);
        }

        private List<SelectListItem> GetRoles() {
            var roles = new List<SelectListItem>();

            foreach (var item in RoleManager.Roles) {
                roles.Add(new SelectListItem {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            return roles;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel viewModel) {
            if (ModelState.IsValid) {
                var user = Mapper.Map<AppUser>(viewModel);
                var result = await UserManager.CreateAsync(user, viewModel.Password);

                if (!result.Succeeded) {
                    ViewBag.Errors = result.Errors.ToList();
                    return View();
                }

                await UserManager.AddToRoleAsync(user.Id, viewModel.RoleText);

                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", "Home", null);
            }
            return View();
        }
        #endregion

        #region Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel, string returnUrl) {
            if (ModelState.IsValid) {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password,
                    loginViewModel.RememberMe, shouldLockout: false);

                if (result == SignInStatus.Success) {
                    return RedirectToLocal(returnUrl);
                }

                //if (result == SignInStatus.LockedOut) {
                //    return View("Lockout");
                //}

                //if (result == SignInStatus.RequiresVerification) {
                //    return RedirectToAction("SendCode",
                //        new { ReturnUrl = returnUrl, RememberMe = loginViewModel.RememberMe });
                //}

                if (result == SignInStatus.Failure) {
                    ModelState.AddModelError("", "Invalid login attempt");
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);
        }

        private ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Logoff
        public ActionResult Logoff() {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region ProfilePage
        [Authorize]
        public ActionResult UserProfile() {
            var currentUser = UserManager.FindById(Convert.ToInt32(User.Identity.GetUserId()));
            UserProfileVm userProfileVm = Mapper.Map<UserProfileVm>(currentUser);

            return View(userProfileVm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UserProfile(UserProfileVm viewModel) {
            AppUser updatedUser = UserManager.MapExistingUserWithVm(UserManager.FindById(viewModel.Id), Mapper.Map<AppUser>(viewModel));

            if (Request.Files[0] != null) {
                using (MemoryStream memory = new MemoryStream()) {
                    Request.Files[0].InputStream.CopyTo(memory);
                    updatedUser.Image = memory.ToArray();
                }
            }

            IdentityResult result = UserManager.Update(updatedUser);

            if (!result.Succeeded) {
                return RedirectToAction("Error", "Error");
            }

            ViewBag.UpdateMessage = "Successfully Updated";
            return View(viewModel);
        }

        public ActionResult GetUserProfilePicture(int id) {
            var user = UserManager.FindById(id);
            var imageData = user.Image;
            return File(imageData, "image/jpg");
        }
        #endregion

        #region helpers
        private IAuthenticationManager AuthenticationManager {
            get {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        #endregion
    }
}