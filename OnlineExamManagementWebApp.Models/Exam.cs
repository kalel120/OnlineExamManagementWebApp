using System.ComponentModel.DataAnnotations;

namespace OnlineExamManagementWebApp.Models {
    public class Exam {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; }

        [Required]        
        public string Topic { get; set; }

        [Required]
        public double FullMarks { get; set; }

        public int Duration { get; set; }

        public Course Course { get; set; }

        public int CourseId { get; set; }
    }
}