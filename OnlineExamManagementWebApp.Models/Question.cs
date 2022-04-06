using System;
using System.Collections.Generic;

namespace OnlineExamManagementWebApp.Models {
    public class Question {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string OptionType { get; set; }
        public int Marks { get; set; }
        public int Serial { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        // Navigation
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public ICollection<QuestionOption> QuestionOptions { get; set; }

    }
}
