using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.DTOs;

namespace OnlineExamManagementWebApp.Repository {
    public class ExamRepository {
        private readonly ApplicationDbContext _dbContext;

        public ExamRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public void SaveAll(List<Exam> examsToBeSaved) {
            _dbContext.Exams.AddRange(examsToBeSaved);
        }

        public List<Exam> GetActiveExamsByCourseId(int courseId) {
            return _dbContext.Exams.Where(e => e.CourseId == courseId && e.IsDeleted == false)
                .OrderBy(e => e.SerialNo)
                .ToList();
        }

        public Exam GetActiveExamByCode(int courseId, string code) {
            var exam = _dbContext.Exams.SingleOrDefault(e => e.CourseId == courseId && e.IsDeleted == false && e.Code == code);
            return exam;
        }

        public Exam GetActiveExamById(int id) {
            var exam = _dbContext.Exams.SingleOrDefault(e => e.Id == id && e.IsDeleted == false);
            return exam;
        }

        public void Update(Exam updatable) {
            _dbContext.Entry(updatable).State = EntityState.Modified;
        }

        public ICollection<ExamIndexPageDto> GetAllExamsForIndex() {
            var exams = _dbContext.Exams.Include(c => c.Course)
                .Select(result => new ExamIndexPageDto {
                    Id = result.Id,
                    Code = result.Code,
                    Type = result.Type,
                    Topic = result.Topic,
                    FullMarks = result.FullMarks,
                    Duration = result.Duration,
                    CourseName = result.Course.Name,
                    CourseId = result.CourseId,
                    OrganizationId = result.Course.OrganizationId,
                    OrganizationName = result.Course.Organization.Name,
                    SerialNo = result.SerialNo,
                    IsDeleted = result.IsDeleted

                })
                .Where(e => e.IsDeleted == false)
                .ToList();

            return exams;
        }


    }
}