using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Repository {
    public class CourseTrainerRepository {
        private readonly ApplicationDbContext _dbContext;

        public CourseTrainerRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public List<CourseTrainer> GetCourseTrainersByCourseId(int id) {
            return _dbContext.CourseTrainers
                .Where(ct => ct.CourseId == id)
                .Include(ct => ct.Trainer)
                .ToList();
        }

        public bool AssignTrainerOfACourse(List<CourseTrainer> courseTrainers) {
            _dbContext.CourseTrainers.AddRange(courseTrainers);
            return _dbContext.SaveChanges() > 0;
        }

        public bool RemoveTrainerAssignment(CourseTrainer removableTrainer) {
            var courseTrainer = _dbContext.CourseTrainers
                .SingleOrDefault(ct=>ct.CourseId ==removableTrainer.CourseId && ct.TrainerId==removableTrainer.TrainerId);

            if (courseTrainer ==null) {
                return false;
            }

            _dbContext.CourseTrainers.Remove(courseTrainer);
            return _dbContext.SaveChanges() > 0;
        }

        public bool UpdateLeadTrainerStatus(CourseTrainer updatable) {
            var courseTrainer = _dbContext.CourseTrainers
                .Where(ct => ct.CourseId == updatable.CourseId && ct.TrainerId == updatable.TrainerId).SingleOrDefault();

            if (courseTrainer == null) {
                return false;
            }

            courseTrainer.IsLead = updatable.IsLead;

            _dbContext.Entry(courseTrainer).State = EntityState.Modified;
            return _dbContext.SaveChanges() > 0;
        }
    }
}
