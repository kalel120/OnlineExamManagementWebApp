using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamManagementWebApp.Models {
    public class Course {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public double Credit { get; set; }

        [Required]
        public string Outline { get; set; }

        public int OrganizationId { get; set; }

        public bool IsDeleted { get; set; }

        public List<int> TagIds { get; set; }

        // Navigation Properties
        public Organization Organization { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Exam> Exams { get; set; }

        public ICollection<CourseTrainer> CourseTrainers { get; set; }
    }
}