using System.Collections.Generic;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;
using System.Data.Entity;
using OnlineExamManagementWebApp.DTOs;

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
            return _dbContext.Courses.FirstOrDefault(c => c.Code == courseCode && c.OrganizationId == organizationId && c.IsDeleted == false) != null;
        }

        public Course GetCourseById(int? id) {
            var course = _dbContext.Courses.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            return course;
        }

        public ICollection<Course> GetListOfCourseByOrganizationId(int organizationId) {
            return _dbContext.Courses
                .Include(c => c.CourseTrainers)
                .Where(c => c.OrganizationId == organizationId && c.IsDeleted == false).ToList();
        }

        public ICollection<Course> GetCoursesByOrgId(int orgId) {
            var query = _dbContext.Courses.Where(c => c.IsDeleted == false && c.OrganizationId == orgId);
            return query.ToList();
        }

        public ICollection<Course> GetCoursesLikeName(string name) {
            return _dbContext.Courses
                .Include(c => c.CourseTrainers)
                .Where(c => c.Name.Contains(name) && c.IsDeleted == false).ToList();
        }

        public ICollection<Course> GetCoursesLikeCode(string code) {
            return _dbContext.Courses
                .Include(c => c.CourseTrainers)
                .Where(c => c.Code.Contains(code) && c.IsDeleted == false).ToList();
        }

        public ICollection<Course> GetAllActiveCourses() {
            return _dbContext.Courses
                .Include(c => c.CourseTrainers)
                .Where(c => c.IsDeleted == false)
                .ToList();
        }

        public ICollection<Course> GetCourseByRangedDuration(int? durationFrom, int? durationTo) {
            if (durationFrom == null) {
                return _dbContext.Courses
                    .Include(c => c.CourseTrainers)
                    .Where(d => d.Duration <= durationTo && d.IsDeleted == false)
                    .ToList();
            }
            if (durationTo == null) {
                return _dbContext.Courses
                    .Include(c => c.CourseTrainers)
                    .Where(d => d.Duration >= durationFrom && d.IsDeleted == false)
                    .ToList();
            }
            return _dbContext.Courses
                .Include(c => c.CourseTrainers)
                .Where(d => d.Duration >= durationFrom && d.Duration <= durationTo && d.IsDeleted == false)
                .ToList();
        }

        public bool IsCourseUpdated(Course course) {
            var updateableCourse = _dbContext.Courses.FirstOrDefault(c => c.Id == course.Id && c.IsDeleted == false);
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

        public bool IsCourseDeleted(int courseId) {
            var deletableCourse = _dbContext.Courses.FirstOrDefault(c => c.Id == courseId && c.IsDeleted == false);
            if (deletableCourse != null) {
                deletableCourse.IsDeleted = true;

                _dbContext.Entry(deletableCourse).State = EntityState.Modified;
            }

            return _dbContext.SaveChanges() > 0;
        }

        public ICollection<CourseWithOrgNameDto> GetAllActiveCoursesWithOrganization() {
            var query = _dbContext.Courses
                .Include(c => c.Organization)
                .Where(c => c.IsDeleted == false);

            return query.Select(c => new CourseWithOrgNameDto {
                CourseId = c.Id,
                CourseName = c.Name,
                CourseCode = c.Code,
                CourseOutline = c.Outline,
                OrganizationId = c.OrganizationId,
                OrganizationName = c.Organization.Name
            }).ToList();
        }
    }
}