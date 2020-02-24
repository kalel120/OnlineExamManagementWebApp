using Microsoft.AspNet.Identity;
using OnlineExamManagementWebApp.Models.Identity;

namespace OnlineExamManagementWebApp.BLL.Identity {
    public class AppUserManager : UserManager<AppUser, int> {
        public AppUserManager(IUserStore<AppUser, int> store) : base(store) {
        }

        public AppUser MapExistingUserWithVm(AppUser updateableUser, AppUser viewModelUser) {
            updateableUser.Name = viewModelUser.Name;
            updateableUser.Email = viewModelUser.Email;
            updateableUser.Address = viewModelUser.Address;
            updateableUser.PhoneNumber = viewModelUser.PhoneNumber;
            updateableUser.City = viewModelUser.City;
            updateableUser.Country = viewModelUser.Country;
            updateableUser.PostalCode = viewModelUser.PostalCode;
            updateableUser.Profession = viewModelUser.Profession;
            updateableUser.HighestAcademic = viewModelUser.HighestAcademic;
            return updateableUser;
        }
    }
}
