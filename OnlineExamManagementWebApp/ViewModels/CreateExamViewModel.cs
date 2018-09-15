using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.ViewModels {
    public class CreateExamViewModel {
        public string OrganizationName { get; set; }

        public Course Course { get; set; }

        public string ExamType { get; set; }

        public string Code { get; set; }

        public string Topic { get; set; }

        public double FullMarks { get; set; }

        public int DurationHour { get; set; }

        public int DurationMin { get; set; }

        public string Serial { get; set; }
    }
}