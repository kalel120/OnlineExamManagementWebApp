using OnlineExamManagementWebApp.DatabaseContext;

namespace OnlineExamManagementWebApp.Repository {
    public class UnitOfWork  {
        private static readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        private CourseRepository _courseRepository = new CourseRepository(_dbContext);
    }
}
