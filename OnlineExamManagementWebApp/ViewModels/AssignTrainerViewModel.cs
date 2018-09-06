using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineExamManagementWebApp.ViewModels {
    public class AssignTrainerViewModel {
        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }

        public List<SelectListItem> Trainers { get; set; }
    }
}