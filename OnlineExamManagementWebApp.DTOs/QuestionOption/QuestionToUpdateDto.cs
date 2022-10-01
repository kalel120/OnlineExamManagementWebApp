using System;

namespace OnlineExamManagementWebApp.DTOs.QuestionOption {
    public class QuestionToUpdateDto {
        public int ExamId { get; set; }
        public Guid QuestionId { get; set; }
        public string OptionType { get; set; }
        public int Marks { get; set; }
        public string Description { get; set; }
    }
}