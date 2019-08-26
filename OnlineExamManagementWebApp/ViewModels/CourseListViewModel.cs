using System.ComponentModel.DataAnnotations;

namespace OnlineExamManagementWebApp.ViewModels {
    public class CourseListViewModel {
        public int CourseId { get; set; }
        [Display(Name = "Organization Name")]
        public string OrganizationName { get; set; }

        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Display(Name = "Course Outline")]
        public string CourseOutline { get; set; }
    }
}