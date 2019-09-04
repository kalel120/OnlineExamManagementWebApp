using System.ComponentModel.DataAnnotations;
using System.Web;

namespace OnlineExamManagementWebApp.ViewModels {
    public class OrgEntryViewModel {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public string Code { get; set; }

        [Required]
        [MaxLength(20)]
        public string Address { get; set; }

        [Required]
        [MaxLength(20)]
        public string Contact { get; set; }

        public string About { get; set; }

        public HttpPostedFileBase Logo { get; set; }
    }
}