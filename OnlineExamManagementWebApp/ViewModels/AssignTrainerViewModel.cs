using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.ViewModels {
    public class AssignTrainerViewModel {

        public int CourseId { get; set; }

        public List<int> Trainers { get; set; }

        public int LeadTrainerId { get; set; }
    }
}