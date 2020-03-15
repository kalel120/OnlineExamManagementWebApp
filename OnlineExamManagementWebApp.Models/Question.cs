using System;
using System.Collections.Generic;

namespace OnlineExamManagementWebApp.Models {
    public class Question {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string OptionType { get; set; }
        public Guid OptionOneId { get; set; }
        public Guid OptionTwoId { get; set; }
        public Guid OptionThreeId { get; set; }
        public Guid OptionFourId { get; set; }
        public int Marks { get; set; }
        public int Serial { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<QuestionOption> QuestionOptions { get; set; }

        // Navigation
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
    }
}
