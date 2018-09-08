using System.Collections.Generic;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {
    public class TrainerManager {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public List<Trainer> GetAllTrainers() {
            return _unitOfWork.TrainerRepository.GetAllTrainers();
        }

        public List<Trainer> GetTrainersByCourseId(int id) {
            return _unitOfWork.TrainerRepository.GetTrainersByCourseId(id);
        }
    }
}
