using System;

namespace OnlineExamManagementWebApp.DTOs.QuestionOption {
    public class SingleOptionToSave {
        public int SerialNo { get; set; }

        public string OptionText { get; set; }

        public bool IsCorrectAnswer { get; set; }

        public int ExamId { get; set; }

        public Guid QuestionId { get; set; }
    }
}