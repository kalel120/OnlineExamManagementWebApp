using System;

namespace OnlineExamManagementWebApp.DTOs.QuestionOption {
    public class OptionToUpdate {
        public Guid OptionId { get; set; }
        public Guid QuestionId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public int Order { get; set; }
        public int ExamId { get; set; }
    }
}
