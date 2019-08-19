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

        public bool IsLeadTrainerStatusUpdatedById(int newLeadTrainerId, int courseId) {
            var courseTrainers = _unitOfWork.CourseTrainers.GetCourseTrainersByCourseId(courseId);
            var existingLeadCourseTrainer = new CourseTrainer();
            var possibleLeadCourseTrainer = new CourseTrainer();

            foreach (var item in courseTrainers) {
                if (item.IsLead) {
                    existingLeadCourseTrainer = item;
                }

                if (item.TrainerId == newLeadTrainerId) {
                    possibleLeadCourseTrainer = item;
                }
            }

            if (existingLeadCourseTrainer.TrainerId == newLeadTrainerId) {
                return true;
            }

            return IsLeadTrainerStatusUpdated(existingLeadCourseTrainer, possibleLeadCourseTrainer);
        }

        private bool IsLeadTrainerStatusUpdated(CourseTrainer existingLeadCourseTrainer, CourseTrainer newLeadCourseTrainer) {
            // delete operation
            existingLeadCourseTrainer.IsLead = false;
            var isRemovedLeadStatus = _unitOfWork.CourseTrainers.UpdateLeadTrainerStatus(existingLeadCourseTrainer);

            // insert operation
            newLeadCourseTrainer.IsLead = true;
            var isUpdateLeadStatus = _unitOfWork.CourseTrainers.UpdateLeadTrainerStatus(newLeadCourseTrainer);

            return isRemovedLeadStatus && isUpdateLeadStatus;
        }
    }
}
