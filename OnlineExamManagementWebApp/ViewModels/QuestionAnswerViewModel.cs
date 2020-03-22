using System.ComponentModel.DataAnnotations;

namespace OnlineExamManagementWebApp.ViewModels {
    public class QuestionAnswerViewModel {
        public int ExamId { get; set; }

        [Display(Name = "Organization")]
        public string OrganizationName { get; set; }

        [Display(Name = "Course")]
        public string CourseName { get; set; }

        [Display(Name = "Exam Code")]
        public string ExamCode { get; set; }
    }
}