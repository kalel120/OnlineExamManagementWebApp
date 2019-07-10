using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExamManagementWebApp.Models {
    public class CourseTrainer {
        [Key, Column(Order = 0)]
        public int CourseId { get; set; }

        [Key, Column(Order = 1)]
        public int TrainerId { get; set; }

        // Navigation Property
        public Course Course { get; set; }
        public Trainer Trainer { get; set; }


        // Custom Attributes
        public bool IsLead { get; set; }
    }
}
