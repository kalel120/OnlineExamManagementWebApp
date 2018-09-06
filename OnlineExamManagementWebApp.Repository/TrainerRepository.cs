using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Repository {
    public class TrainerRepository {
        private readonly ApplicationDbContext _dbContext;

        public TrainerRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public List<Trainer> GetAllTrainers() {
            return _dbContext.Trainers.ToList();
        }
    }
}
