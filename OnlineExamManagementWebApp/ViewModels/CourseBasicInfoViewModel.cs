using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineExamManagementWebApp.ViewModels {
    public class CourseBasicInfoViewModel {
        [Display(Name = "Organization")]
        public string OrganizationCode { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int Duration { get; set; }

        public double Credit { get; set; }

        [AllowHtml]
        public string Outline { get; set; }

        public AssignTrainerViewModel AssignTrainerVm { get; set; }
    }
}