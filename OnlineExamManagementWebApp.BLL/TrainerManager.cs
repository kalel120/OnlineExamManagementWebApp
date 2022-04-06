using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {
    public class TrainerManager {
        private readonly UnitOfWork _unitOfWork;

        public TrainerManager() {
            _unitOfWork = new UnitOfWork();
        }

        public List<Trainer> GetTrainersByOrgId(int orgId) {
            return _unitOfWork.Trainers.GetTrainersByOrgId(orgId);
        }

        public List<SelectListItem> GetSelectListTrainersByOrgId(string orgId) {
            List<Trainer> repoTrainers = GetTrainersByOrgId(Convert.ToInt32(orgId));

            List<SelectListItem> trainers = repoTrainers.Select(trainer => new SelectListItem {
                Value = trainer.Id.ToString(),
                Text = trainer.Name
            })
                .ToList();
            trainers.Insert(0, new SelectListItem { Value = "", Text = @"--SELECT TRAINER--" });
            return trainers;
        }

        public List<SelectListItem> GetEmptySelectList() {
            var trainers = new List<SelectListItem>();
            trainers.Insert(0, new SelectListItem { Value = "", Text = @"--SELECT TRAINER--" });
            return trainers;
        }

        public bool IsTrainerDeleted(int id) {
            var trainer = _unitOfWork.Trainers.GetTrainerById(id);
            trainer.IsDeleted = true;
            return _unitOfWork.Complete();
        }
    }
}
