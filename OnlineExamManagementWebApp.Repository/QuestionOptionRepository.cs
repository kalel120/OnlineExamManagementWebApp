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
            try {
                ICollection<OptionDto> result = _dbContext.QuestionOptions
                    .Where(qo => qo.QuestionId == questionId && qo.IsDeleted == false)
                    .Select(qo => new OptionDto {
                        OptionId = qo.OptionId,
                        Order = qo.Order,
                        DateCreated = qo.Option.DateCreated,
                        Description = qo.Option.Description,
                        IsMarkedAsAnswer = qo.IsCorrectAnswer
                    })
                    .ToList();
                return result;
            }
            catch (Exception e) {
                return (ICollection<OptionDto>)Enumerable.Empty<OptionDto>();
            }
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

        public bool IsOptionRemoved(Guid optionId, int examId) {
            bool result;
            try {
                var optionToRemove = _dbContext.QuestionOptions
                    .FirstOrDefault(qo => qo.ExamId == examId && qo.OptionId == optionId);

                if (optionToRemove != null) {
                    optionToRemove.IsDeleted = true;
                    _dbContext.Entry(optionToRemove).State = EntityState.Modified;
                }

                result = _dbContext.SaveChanges() > 0;
            }
            catch (Exception e) {
                throw new NotImplementedException();
            }
            return result;
        }
    }
}
