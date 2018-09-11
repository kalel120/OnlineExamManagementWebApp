namespace OnlineExamManagementWebApp.DTOs {
    public class CourseTrainerDto {
        public int CourseId { get; set; }
        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public bool IsLead { get; set; }
    }
}