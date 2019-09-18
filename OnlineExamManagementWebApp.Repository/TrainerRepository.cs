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

        public List<Trainer> GetTrainersByOrgId(int orgId) {
            return _dbContext.Trainers.Where(t => t.OrganizationId == orgId && t.IsDeleted == false).ToList();
        }

        public Trainer GetTrainersById(int id) {
            return _dbContext.Trainers.Single(t => t.Id == id && t.IsDeleted == false);
        }
    }
}
