using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {
    public class CourseTrainerManager {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public List<CourseTrainer> GetCourseTrainersByCourseId(int id) {
            return _unitOfWork.CourseTrainerRepository.GetCourseTrainersByCourseId(id);
        }

        public bool AssignTrainerOfACourse(List<CourseTrainer> courseTrainers) {
            return _unitOfWork.CourseTrainerRepository.AssignTrainerOfACourse(courseTrainers);
        }

        public bool RemoveTrainerAssignment(CourseTrainer removableTrainer) {
            return _unitOfWork.CourseTrainerRepository.RemoveTrainerAssignment(removableTrainer);
        }

        public bool UpdateLeadTrainerStatus(CourseTrainer updatable) {
            return _unitOfWork.CourseTrainerRepository.UpdateLeadTrainerStatus(updatable);
        }
    }
}
