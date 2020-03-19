﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OnlineExamManagementWebApp.DTOs;

namespace OnlineExamManagementWebApp.ViewModels {
    public class QuestionAnswerViewModel {
        [Display(Name = "Organization")]
        public string OrganizationName { get; set; }

        [Display(Name = "Course")]
        public string CourseName { get; set; }

        [Display(Name = "Exam Code")]
        public string ExamCode { get; set; }

        public ICollection<QuestionsDto> Questions { get; set; }
    }
}