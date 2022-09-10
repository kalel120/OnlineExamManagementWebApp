using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.DTOs.QuestionOption;
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

        public ICollection<OptionDto> GetOptionsByQuestionId(Guid questionId) {
            ICollection<OptionDto> result = _dbContext.QuestionOptions
                .Where(qo => qo.QuestionId == questionId && qo.IsDeleted == false)
                //.Include(qo => qo.Option)
                //.Select(qo => qo.Option)
                .Select(oDto => new OptionDto {
                    OptionId = oDto.OptionId,
                    DateCreated = oDto.Option.DateCreated,
                    Description = oDto.Option.Description,
                    IsMarkedAsAnswer = oDto.IsCorrectAnswer
                })
                .ToList();

            return result;
        }
    }
}
