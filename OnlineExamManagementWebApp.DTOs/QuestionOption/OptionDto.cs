using System;

namespace OnlineExamManagementWebApp.DTOs.QuestionOption {
    public class OptionDto {
        public Guid OptionId { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public bool IsMarkedAsAnswer { get; set; }
        public DateTime DateCreated { get; set; }
    }
}