using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using OnlineExamManagementWebApp.BLL.Identity;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models.Identity;
using Owin;
using System;

namespace OnlineExamManagementWebApp.Config {
    public static class AppConfiguration {

        public static void AuthConfiguration(IAppBuilder app) {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(CreateUserManager);
            app.CreatePerOwinContext<AppSignInManager>(CreateSignInManager);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    //OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<AppUserManager, AppUser,int>(
                    //    validateInterval: TimeSpan.FromMinutes(30),
                    //    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        private static AppSignInManager CreateSignInManager(IdentityFactoryOptions<AppSignInManager> options, IOwinContext context) {
            return new AppSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        }

        private static AppUserManager CreateUserManager(IdentityFactoryOptions<AppUserManager> options, IOwinContext context) {
            var manager = new AppUserManager(new UserStore<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>(new ApplicationDbContext()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<AppUser, int>(manager) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            //manager.PasswordValidator = new PasswordValidator {
            //    RequiredLength = 6,
            //    RequireNonLetterOrDigit = true,
            //    RequireDigit = true,
            //    RequireLowercase = true,
            //    RequireUppercase = true,
            //};

            // Validation logic for password
            manager.PasswordValidator = new PasswordValidator {
                RequiredLength = 5
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            return manager;
        }


    }
}
