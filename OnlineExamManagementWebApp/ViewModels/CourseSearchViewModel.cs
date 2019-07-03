using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.ViewModels {
    public class CourseSearchViewModel {
        [Display(Name = "Organization")]
        public int OrganizationId { get; set; }

        public List<SelectListItem> Organizations { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        [Display(Name = "Duration From")]
        public int DurationFrom { get; set; }

        [Display(Name = "To")]
        public int DurationTo { get; set; }

        public int TrainerId { get; set; }

        public List<SelectListItem> Trainers { get; set; }

        public List<Course> Courses { get; set; }
    }
}