using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExamManagementWebApp.Models {
    public class CourseTrainer {

        public Course Course { get; set; }

        public Trainer Trainer { get; set; }

        [Key,Column(Order = 1)]
        public int CourseId { get; set; }

        [Key, Column(Order = 2)]
        public int TrainerId { get; set; }

        public bool IsLead { get; set; }
    }
}
