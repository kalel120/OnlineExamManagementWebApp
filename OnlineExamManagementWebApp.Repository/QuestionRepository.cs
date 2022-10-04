using System;
using System.Collections.Generic;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Repository {
    public class QuestionRepository {
        private readonly ApplicationDbContext _dbContext;

        public QuestionRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        // Returns Question which is not deleted
        public Question GetSingleQuestionById(Guid questionId) {
            Question question = _dbContext.Questions.SingleOrDefault(q => q.Id == questionId && q.IsDeleted == false);

            return question;
        }

        public IEnumerable<Question> GetQuestionsById(Guid questionId) {
            var questions = _dbContext.Questions.Where(q => q.Id == questionId && q.IsDeleted == false);

            return questions;
        }
    }
}
