using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                ICollection<QuestionsDto> result = _dbContext.Questions
                    .Where(q => q.ExamId == examId && q.IsDeleted == false)
                    .Include(q => q.QuestionOptions)
                    .Select(dto => new QuestionsDto {
                        QuestionId = dto.Id,
                        Serial = dto.Serial,
                        OptionType = dto.OptionType,
                        Description = dto.Description,
                        QuestionOption = dto.QuestionOptions
                    })
                    .ToList();

                return result;
            }
            catch (Exception e) {
                return (ICollection<QuestionsDto>)Enumerable.Empty<QuestionsDto>();
            }
        }
    }
}
