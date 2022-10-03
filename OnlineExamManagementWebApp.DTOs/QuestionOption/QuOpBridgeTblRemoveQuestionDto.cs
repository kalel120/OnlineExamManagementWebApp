using System;

namespace OnlineExamManagementWebApp.DTOs.QuestionOption {
    public class QuOpBridgeTblRemoveQuestionDto {
        public Guid QuestionId { get; set; }
        public int ExamId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
