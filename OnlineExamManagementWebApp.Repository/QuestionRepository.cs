using System;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Repository {
    public class QuestionRepository {
        private readonly ApplicationDbContext _dbContext;

        public QuestionRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        // Retuns Question which is not deleted
        public Question GetQuestionById(Guid questionId) {
            Question question = _dbContext.Questions.SingleOrDefault(q => q.Id == questionId && q.IsDeleted == false);

            return question;
        }
    }
}
