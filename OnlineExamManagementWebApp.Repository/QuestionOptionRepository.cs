using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.DTOs;

namespace OnlineExamManagementWebApp.Repository {
    public class QuestionOptionRepository {
        private readonly ApplicationDbContext _dbContext;
        public QuestionOptionRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public ICollection<QuestionsDto> GetActiveQuestions(int examId) {
            var query = _dbContext.QuestionOptions
                .Include(qo => qo.Question)
                .Include(qo => qo.Option)
                .Where(qo => qo.IsDeleted == false && qo.Question.ExamId == examId)
                .Select(q => new QuestionsDto {
                    QuestionId = q.QuestionId,
                    Serial = q.Question.Serial,
                    OptionType = q.Question.OptionType
                }).ToList();
            return query;
        }
    }
}
