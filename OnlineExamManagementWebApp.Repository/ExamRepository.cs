using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                .OrderBy(e=>e.SerialNo)
                .ToList();
        }

        public bool SaveAll(List<Exam> examsToBeSaved) {
             _dbContext.Exams.AddRange(examsToBeSaved);
            return _dbContext.SaveChanges() > 0;
        }


        public bool UpdateSerialNo() {
            return _dbContext.SaveChanges() > 0;
        }
    }
}
