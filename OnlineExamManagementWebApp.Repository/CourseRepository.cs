using System.Collections.Generic;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;
using System.Data.Entity;
namespace OnlineExamManagementWebApp.Repository {
    public class CourseRepository {
        private readonly ApplicationDbContext _dbContext;

        public CourseRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        private IQueryable<Course> EgarLoadCoursesWithTrainerObject() {
            return _dbContext.Courses.Include(c => c.CourseTrainers.Select(t => t.Trainer));
        }

        public bool AddCourse(Course course) {
            _dbContext.Courses.Add(course);
            return _dbContext.SaveChanges() > 0;
        }

        public bool IsDuplicateCode(string courseCode, int organizationId) {
            return _dbContext.Courses.FirstOrDefault(c => c.Code == courseCode & c.OrganizationId == organizationId) != null;
        }

        public Course GetCourseById(int? id) {
            var course = _dbContext.Courses.FirstOrDefault(c => c.Id == id);
            return course;
        }

        public ICollection<Course> GetListOfCourseByOrganizationId(int organizationId, bool isNeedTrainer) {
            if (isNeedTrainer) {
                return EgarLoadCoursesWithTrainerObject().Where(o => o.OrganizationId == organizationId).ToList();
            }
            return _dbContext.Courses.Where(c => c.OrganizationId == organizationId).ToList();
        }


        public ICollection<Course> GetCoursesLikeName(string name, bool isNeedTrainer) {
            if (isNeedTrainer) {
                return EgarLoadCoursesWithTrainerObject().Where(c => c.Name.Contains(name)).ToList();
            }
            return _dbContext.Courses.Where(c => c.Name.Contains(name)).ToList();
        }

        public ICollection<Course> GetCoursesLikeCode(string code, bool isNeedTrainer) {
            if (isNeedTrainer) {
                return EgarLoadCoursesWithTrainerObject().Where(c => c.Code.Contains(code)).ToList();
            }
            return _dbContext.Courses.Where(c => c.Code.Contains(code)).ToList();
        }

        public ICollection<Course> GetAllCourses(bool isNeedTrainer) {
            if (isNeedTrainer) {
                return EgarLoadCoursesWithTrainerObject().ToList();
            }
            return _dbContext.Courses.ToList();
        }
    }
}
