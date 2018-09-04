using OnlineExamManagementWebApp.DatabaseContext;

namespace OnlineExamManagementWebApp.Repository {
    public class CourseRepository {
        private ApplicationDbContext _dbContext;
        public CourseRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }
    }
}
