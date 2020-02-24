using Microsoft.AspNet.Identity;
using OnlineExamManagementWebApp.Models.Identity;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL.Identity {
    public class AppUserManager : UserManager<AppUser, int> {
        private readonly UnitOfWork _unitOfWork;

        public AppUserManager(IUserStore<AppUser, int> store) : base(store) {
            _unitOfWork = new UnitOfWork();
        }

        public AppUser MapExistingUserWithVm(AppUser updateableUser, AppUser viewModelUser) {
            updateableUser.Name = viewModelUser.Name;
            updateableUser.Email = viewModelUser.Email;
            updateableUser.Address = viewModelUser.Address;
            updateableUser.City = viewModelUser.City;
            updateableUser.Country = viewModelUser.Country;
            updateableUser.PostalCode = viewModelUser.PostalCode;
            updateableUser.Profession = viewModelUser.Profession;
            updateableUser.HighestAcademic = viewModelUser.HighestAcademic;

            return updateableUser;
        }
    }
}
