using System.Data.Entity;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Repository {
    public class CourseRepository {
        private readonly ApplicationDbContext _dbContext;

        public CourseRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }
        
        public bool AddCourse(Course course) {
            _dbContext.Courses.Add(course);
            return _dbContext.SaveChanges() > 0;
        }

        public bool IsDuplicateCode(string courseCode, int organizationId) {
            return _dbContext.Courses.Where(c => c.Code == courseCode & c.OrganizationId== organizationId).FirstOrDefault() != null;
        }

        public Course GetCourseById(int? id) {
            return _dbContext.Courses.Where(c=>c.Id == id).Include(c=>c.Exams).FirstOrDefault();                
        }
    }
}
