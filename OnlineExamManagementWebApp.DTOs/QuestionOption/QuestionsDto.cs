using System;
using System.Collections.Generic;

namespace OnlineExamManagementWebApp.DTOs.QuestionOption {
    public class QuestionsDto {
        public Guid QuestionId { get; set; }
        public int Serial { get; set; }
        public int Marks { get; set; }
        public string OptionType { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public ICollection<Models.QuestionOption> QuestionOption { get; set; }
        public ICollection<OptionDto> Options { get; set; }
    }
}
