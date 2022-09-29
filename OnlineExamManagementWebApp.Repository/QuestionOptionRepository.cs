using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.DTOs.QuestionOption;
using OnlineExamManagementWebApp.Models;
using EntityState = System.Data.Entity.EntityState;

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
                    .OrderBy(o => o.Order)
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

                var questionOptions = _dbContext.QuestionOptions
                    .Where(qo => qo.ExamId == examId && qo.IsDeleted == false)
                    .Distinct()
                    .Select(x => new { x.QuestionId, x.OptionId });

                var result = new List<QuestionsDto>();

                foreach (var item in questions) {
                    result.Add(new QuestionsDto {
                        QuestionId = item.Id,
                        Serial = item.Serial,
                        Marks = item.Marks,
                        OptionType = item.OptionType,
                        Description = item.Description,
                        DateCreated = item.DateCreated,
                        OptionCount = questionOptions.Count(s => s.QuestionId == item.Id)
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

        public bool IsOptionReordered(ICollection<OptionToUpdate> dto, int examId, Guid questionId) {
            var dtoList = dto.ToList();

            var qoList = _dbContext.QuestionOptions
                .Where(qo => qo.ExamId == examId && qo.QuestionId == questionId && qo.IsDeleted == false)
                .ToList();


            for (int index = 0; index < qoList.Count; index++) {
                if (dtoList[index].OptionId == qoList[index].OptionId) {
                    qoList[index].Order = dtoList[index].Order;

                    _dbContext.Entry(qoList[index]).State = EntityState.Modified;
                }
            }

            var result = _dbContext.SaveChanges() > 0;
            return result;
        }

        public bool IsSingleOptionSaved(SingleOptionToSave dtoOptionToSave) {
            // Create a single Option. Fetch existing question with id.
            // Create single QuestionOption. Bind question and option to this and save it. 
            // This will auto create data on Option table

            DateTime currentDateTime = DateTime.Now;
            Option singleOption = new Option {
                Id = Guid.NewGuid(),
                DateCreated = currentDateTime,
                Description = dtoOptionToSave.OptionText,
                IsDeleted = false,
                DateUpdated = null,
            };

            QuestionOption questionOption = new QuestionOption {
                DateCreated = currentDateTime,
                DateUpdated = null,
                ExamId = dtoOptionToSave.ExamId,
                IsCorrectAnswer = dtoOptionToSave.IsCorrectAnswer,
                Option = singleOption,
                IsDeleted = false,
                OptionId = singleOption.Id,
                Order = dtoOptionToSave.SerialNo,
                Question = _dbContext.Questions.SingleOrDefault(q => q.Id == dtoOptionToSave.QuestionId && q.IsDeleted == false)
            };

            _dbContext.QuestionOptions.Add(questionOption);

            var result = _dbContext.SaveChanges() > 0;
            return result;
        }

        public IEnumerable<QuestionOption> GetRowsByQuestionAndExamId(Guid questionId, int examId) {
            IEnumerable<QuestionOption> questionOptions = _dbContext.QuestionOptions
                .Where(qo => qo.ExamId == examId && qo.QuestionId == questionId && qo.IsDeleted == false);

            return questionOptions;
        }

        public QuestionOption GetRowForSingleOptionById(Guid optionId) {
            return _dbContext.QuestionOptions.SingleOrDefault(qo => qo.OptionId == optionId && qo.IsDeleted == false);
        }
    }
}
