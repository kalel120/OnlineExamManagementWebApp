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
            return _dbContext.CourseTrainers
                .Where(ct => ct.CourseId == id)
                .Select(ct => ct.Trainer)
                .ToList();
        }
    }
}
