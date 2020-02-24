using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamManagementWebApp.ViewModels.Account {
    public class UserProfileVm {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        public string Profession { get; set; }

        [Display(Name = "Highest Academic")]
        public string HighestAcademic { get; set; }

        public HttpPostedFileBase Image { get; set; }
    }
}
