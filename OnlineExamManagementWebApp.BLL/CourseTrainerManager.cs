using System.Collections.Generic;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {
    public class CourseTrainerManager {
        private readonly UnitOfWork _unitOfWork;

        public CourseTrainerManager() {
            _unitOfWork = new UnitOfWork();
        }

        public List<CourseTrainer> GetCourseTrainersByCourseId(int id) {
            return _unitOfWork.CourseTrainers.GetCourseTrainersByCourseId(id);
        }

        public bool AssignTrainerOfACourse(List<CourseTrainer> courseTrainers) {
            return _unitOfWork.CourseTrainers.AssignTrainerOfACourse(courseTrainers);
        }

        public bool RemoveTrainerAssignment(CourseTrainer removableTrainer) {
            return _unitOfWork.CourseTrainers.RemoveTrainerAssignment(removableTrainer);
        }

        public bool UpdateLeadTrainerStatus(CourseTrainer updatable) {
            return _unitOfWork.CourseTrainers.UpdateLeadTrainerStatus(updatable);
        }
    }
}
