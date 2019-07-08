using System.Collections.Generic;
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
            return _dbContext.Courses.FirstOrDefault(c => c.Code == courseCode & c.OrganizationId == organizationId) != null;
        }

        public Course GetCourseById(int? id) {
            var course = _dbContext.Courses.FirstOrDefault(c => c.Id == id);
            return course;
        }

        public ICollection<Course> GetListOfCourseByOrganizationId(int organizationId) {
            return _dbContext.Courses.Where(c => c.OrganizationId == organizationId).ToList();
        }

        public ICollection<Course> GetCoursesByName(string name) {
            return _dbContext.Courses.Where(c => c.Name == name).ToList();
        }

        public ICollection<Course> GetCoursesByCode(string code) {
            return _dbContext.Courses.Where(c => c.Code == code).ToList();
        }

        public ICollection<Course> GetCoursesLikeName(string name) {
            return _dbContext.Courses.Where(c => c.Name.Contains(name)).ToList();
        }

        public ICollection<Course> GetCoursesLikeCode(string code) {
            return _dbContext.Courses.Where(c => c.Code.Contains(code)).ToList();
        }

        public ICollection<Course> GetAllCourses() {
            return _dbContext.Courses.ToList();
        }
    }
}
