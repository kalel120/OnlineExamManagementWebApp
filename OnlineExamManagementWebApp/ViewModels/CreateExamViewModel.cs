using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.ViewModels {
    public class CreateExamViewModel {
        public string OrganizationName { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public string ExamType { get; set; }

        public string Code { get; set; }

        public string Topic { get; set; }

        public double? FullMarks { get; set; }

        public int? DurationHour { get; set; }

        public int? DurationMin { get; set; }

        public int SerialNo { get; set; }

        public bool IsDeleted { get; set; }
    }
}