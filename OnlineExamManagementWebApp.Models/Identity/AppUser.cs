using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineExamManagementWebApp.Models.Identity {
    public class AppUser : IdentityUser<int, AppUserLogin, AppUserRole, AppUserClaim> {

        public string Name { get; set; }
        public string Address { get; set; }
        public string AlternateAddress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Profession { get; set; }
        public string HighestAcademic { get; set; }
        public byte[] Image { get; set; }
    }
}
