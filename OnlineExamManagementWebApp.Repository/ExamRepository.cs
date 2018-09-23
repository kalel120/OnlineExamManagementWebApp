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

        public List<Exam> GetActiveExamsByCourseId(int courseId) {
            return _dbContext.Exams.Where(e => e.CourseId == courseId && e.IsDeleted == false)
                .OrderBy(e => e.SerialNo)
                .ToList();
        }

        public bool SaveAll(List<Exam> examsToBeSaved) {
            _dbContext.Exams.AddRange(examsToBeSaved);
            return _dbContext.SaveChanges() > 0;
        }


        public bool UpdateSerialNo() {
            return _dbContext.SaveChanges() > 0;
        }

        public bool RemoveExamByCode(string examCode, int courseId) {
            var removable = _dbContext.Exams.SingleOrDefault(e => e.CourseId==courseId && e.Code == examCode);
            if (removable == null) {
                return false;
            }
            removable.IsDeleted = true;
            _dbContext.Entry(removable).State = EntityState.Modified;

            return _dbContext.SaveChanges() > 0;
        }
    }
}
