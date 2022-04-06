using System;
using System.Collections.Generic;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.DTOs {
    public class QuestionsDto {
        public Guid QuestionId { get; set; }
        public int Serial { get; set; }
        public string OptionType { get; set; }
        public string Description { get; set; }
        public ICollection<QuestionOption> QuestionOption { get; set; }
    }
}
