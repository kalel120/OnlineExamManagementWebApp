using System.Collections.Generic;
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
            return _dbContext.Courses.FirstOrDefault(c => c.Code == courseCode & c.OrganizationId == organizationId) != null;
        }

        public Course GetCourseById(int? id) {
            var course = _dbContext.Courses.FirstOrDefault(c => c.Id == id);
            return course;
        }

        public List<Course> GetListOfCourseByOrganizationId(int organizationId) {
            return _dbContext.Courses.Where(c => c.OrganizationId == organizationId).ToList();
        }
    }
}
