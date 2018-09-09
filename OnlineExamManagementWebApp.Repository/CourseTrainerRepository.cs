using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
