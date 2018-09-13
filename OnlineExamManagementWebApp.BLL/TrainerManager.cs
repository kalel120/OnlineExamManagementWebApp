using System.Collections.Generic;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {
    public class TrainerManager {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public List<Trainer> GetTrainersByOrgId(int orgId) {
            return _unitOfWork.TrainerRepository.GetTrainersByOrgId(orgId);
        }        
    }
}
