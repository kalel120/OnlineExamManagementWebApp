using System.Collections.Generic;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Repository {
    public class QuestionOptionRepository {
        private readonly ApplicationDbContext _dbContext;
        public QuestionOptionRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public bool IsQuestionAnswerSaved(ICollection<QuestionOption> questionAndOptionsToSave) {
            _dbContext.QuestionOptions.AddRange(questionAndOptionsToSave.ToList());
            bool result = _dbContext.SaveChanges() > 0;
            return result;
        }
    }
}
