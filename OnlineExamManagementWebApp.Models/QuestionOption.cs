﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExamManagementWebApp.Models {
    public class QuestionOption {
        [Key, Column(Order = 0)]
        public Guid QuestionId { get; set; }

        [Key, Column(Order = 1)]
        public Guid OptionId { get; set; }

        public int Order { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsCorrectAnswer { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public int ExamId { get; set; }

        // Navigation
        public virtual Question Question { get; set; }
        public virtual Option Option { get; set; }
    }
}
