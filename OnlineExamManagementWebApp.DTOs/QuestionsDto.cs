using System;

namespace OnlineExamManagementWebApp.DTOs {
    public class QuestionsDto {
        public Guid QuestionId { get; set; }
        public int Serial { get; set; }
        public string OptionType { get; set; }
    }
}
