using System.ComponentModel.DataAnnotations;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.ViewModels {
    public class CreateExamViewModel {
        [Display(Name = "Organization")]
        public string OrganizationName { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public string Type { get; set; }
        
        public string Code { get; set; }

        public string Topic { get; set; }

        [Display(Name = "Full Marks")]
        public double? FullMarks { get; set; }

        public int Duration { get; set; }

        public int? DurationHour { get; set; }

        public int? DurationMin { get; set; }

        [Display(Name = "Serial")]
        public int SerialNo { get; set; }

        public bool IsDeleted { get; set; }
    }
}