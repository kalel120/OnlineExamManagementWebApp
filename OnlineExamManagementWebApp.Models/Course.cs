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
        
        public List<Tag> Tags { get; set; }

        public List<Trainer> Trainers { get; set; }

        public List<Exam> Exams { get; set; }

        public Organization Organization { get; set; }

        public int OrganizationId { get; set; }

        public int? LeadTrainerId { get; set; }
    }
}