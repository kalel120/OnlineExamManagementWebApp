using System;
using System.Collections.Generic;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.DTOs.QuestionOption;

namespace OnlineExamManagementWebApp.Repository {
    public class QuestionRepository {
        private readonly ApplicationDbContext _dbContext;

        public QuestionRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public ICollection<QuestionsDto> GetQuestionsByExamId(int examId) {
            try {
                var questions = _dbContext.QuestionOptions
                    .Where(qo => qo.ExamId == examId && qo.IsDeleted == false)
                    .Select(qo => qo.Question)
                    .Distinct()
                    .ToList();

                var result = new List<QuestionsDto>();

                foreach (var item in questions) {
                    result.Add(new QuestionsDto {
                        QuestionId = item.Id,
                        Serial = item.Serial,
                        Marks = item.Marks,
                        OptionType = item.OptionType,
                        Description = item.Description,
                        DateCreated = item.DateCreated,
                        OptionCount = item.QuestionOptions.Select(qo => qo.Option).Count()
                    });
                }

                return result;
            }
            catch (Exception e) {
                return (ICollection<QuestionsDto>)Enumerable.Empty<QuestionsDto>();
            }
        }
    }
}
