using OnlineExamManagementWebApp.DatabaseContext;

namespace OnlineExamManagementWebApp.Repository {
    public class UnitOfWork {
        private readonly ApplicationDbContext _dbContext;

        public CourseRepository Courses { get; private set; }
        public TagRepository Tags { get; private set; }
        public OrganizationRepository Organizations { get; private set; }
        public TrainerRepository Trainers { get; private set; }
        public CourseTrainerRepository CourseTrainers { get; private set; }
        public ExamRepository Exams { get; private set; }

        public UnitOfWork() {
            _dbContext = new ApplicationDbContext();
            Courses = new CourseRepository(_dbContext);
            Tags = new TagRepository(_dbContext);
            Organizations = new OrganizationRepository(_dbContext);
            Trainers = new TrainerRepository(_dbContext);
            CourseTrainers = new CourseTrainerRepository(_dbContext);
            Exams = new ExamRepository(_dbContext);
        }

        public bool Complete() {
            return _dbContext.SaveChanges() > 0;
        }
    }
}
