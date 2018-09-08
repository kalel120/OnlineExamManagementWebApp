using System.Collections.Generic;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Repository {
    public class TrainerRepository {
        private readonly ApplicationDbContext _dbContext;

        public TrainerRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public List<Trainer> GetAllTrainers(int orgId) {
            return _dbContext.Trainers.Where(t=>t.OrganizationId==orgId).ToList();
        }

        public List<Trainer> GetTrainersByCourseId(int id) {
            //var listTrainerId = from trainers in _dbContext.Trainers
            //    where trainers.Courses.Any(c => c.Id == id)
            //    select trainers;

            return _dbContext.Trainers.Where(t => t.Courses.Any(c => c.Id == id)).ToList();
        }
    }
}
