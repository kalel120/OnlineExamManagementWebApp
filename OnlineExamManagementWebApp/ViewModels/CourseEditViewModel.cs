using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineExamManagementWebApp.ViewModels {
    public class CourseEditViewModel {
        public int Id { get; set; }

        public int OrganizationId { get; set; }

        [Display(Name = "Organization")]
        public string OrganizationCode { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int Duration { get; set; }

        public double Credit { get; set; }

        [AllowHtml]
        public string Outline { get; set; }
        
    }
}