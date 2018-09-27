using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;

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

        public Exam GetCourseSpeceficActiveExamByCode(int examCourseId, string examCode) {
            return _dbContext.Exams.Where(e => e.CourseId == examCourseId && e.IsDeleted == false)
                .SingleOrDefault(e => e.Code == examCode);
        }

        public void Update(Exam updatable) {
            _dbContext.Entry(updatable).State = EntityState.Modified;            
        }        
    }
}
