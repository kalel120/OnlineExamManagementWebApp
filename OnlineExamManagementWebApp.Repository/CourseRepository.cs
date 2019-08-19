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
            return _dbContext.Courses
                .Include(c => c.CourseTrainers)
                .Where(c => c.OrganizationId == organizationId).ToList();
        }

        public ICollection<Course> GetCoursesLikeName(string name) {
            return _dbContext.Courses
                .Include(c => c.CourseTrainers)
                .Where(c => c.Name.Contains(name)).ToList();
        }

        public ICollection<Course> GetCoursesLikeCode(string code) {
            return _dbContext.Courses
                .Include(c => c.CourseTrainers)
                .Where(c => c.Code.Contains(code)).ToList();
        }

        public ICollection<Course> GetAllCourses() {
            return _dbContext.Courses
                .Include(c => c.CourseTrainers)
                .ToList();
        }

        public ICollection<Course> GetCourseByRangedDuration(int? durationFrom, int? durationTo) {
            if (durationFrom == null) {
                return _dbContext.Courses
                    .Include(c => c.CourseTrainers)
                    .Where(d => d.Duration <= durationTo)
                    .ToList();
            }
            if (durationTo == null) {
                return _dbContext.Courses
                    .Include(c => c.CourseTrainers)
                    .Where(d => d.Duration >= durationFrom)
                    .ToList();
            }
            return _dbContext.Courses
                .Include(c => c.CourseTrainers)
                .Where(d => d.Duration >= durationFrom && d.Duration <= durationTo)
                .ToList();
        }

        public bool IsCourseUpdated(Course course) {
            var updateableCourse = _dbContext.Courses.FirstOrDefault(c => c.Id == course.Id);
            if (updateableCourse == null) {
                return false;
            }

            updateableCourse.Name = course.Name;
            updateableCourse.Code = course.Code;
            updateableCourse.Credit = course.Credit;
            updateableCourse.Duration = course.Duration;
            updateableCourse.Outline = course.Outline;

            _dbContext.Entry(updateableCourse).State = EntityState.Modified;
            return _dbContext.SaveChanges() > 0;
        }
    }
}
