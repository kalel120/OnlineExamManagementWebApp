using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineExamManagementWebApp.DTOs;

namespace OnlineExamManagementWebApp.ViewModels {
    public class QuestionAnswerViewModel {
        public string OrganizationName { get; set; }
        public string CourseName { get; set; }
        public string ExamCode { get; set; }
        public string QuestionDescription { get; set; }

        public ICollection<QuestionsDto> Questions { get; set; }
    }
}