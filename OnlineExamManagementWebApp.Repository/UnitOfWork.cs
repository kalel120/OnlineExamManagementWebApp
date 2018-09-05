using OnlineExamManagementWebApp.DatabaseContext;

namespace OnlineExamManagementWebApp.Repository {
    public class UnitOfWork {
        private static readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        public CourseRepository _courseRepository = new CourseRepository(_dbContext);
        public TagRepository _tagRepository = new TagRepository(_dbContext);
        public OrganizationRepository _orgRepository = new OrganizationRepository(_dbContext);
    }
}
