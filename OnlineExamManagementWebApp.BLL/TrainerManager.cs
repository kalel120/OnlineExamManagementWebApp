using System.Collections.Generic;
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
    }
}
