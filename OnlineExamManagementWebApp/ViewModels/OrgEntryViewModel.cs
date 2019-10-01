using System.ComponentModel.DataAnnotations;
using System.Web;

namespace OnlineExamManagementWebApp.ViewModels {
    public class OrgEntryViewModel {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [MaxLength(100)]
        public string Contact { get; set; }

        public string About { get; set; }

        public HttpPostedFileBase Logo { get; set; }
    }
}