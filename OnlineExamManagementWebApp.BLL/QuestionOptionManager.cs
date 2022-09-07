using System;
using System.Collections.Generic;
using OnlineExamManagementWebApp.Repository;
using OnlineExamManagementWebApp.DTOs.QuestionOption;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.BLL {
    public class QuestionOptionManager {
        private readonly UnitOfWork _unitOfWork;

        public QuestionOptionManager() {
            _unitOfWork = new UnitOfWork();
        }

        public ICollection<QuestionsDto> GetQuestionsByExamId(int examId) {
            return _unitOfWork.Questions.GetQuestionsByExamId(examId);
        }

        public bool IsQuestionAnswerSaved(QuestionToSaveDto questionToSaveDto, ICollection<OptionToSaveDto> optionsToSaveDto) {
            Guid questionGuid = Guid.NewGuid();
            DateTime currentDateTime = DateTime.Now;
            ICollection<Option> optionsToSave = new List<Option>();
            ICollection<QuestionOption> questionAndOptionsToSave = new List<QuestionOption>();

            // Mapping dto data to Model data
            Question questionToSave = new Question {
                Id = questionGuid,
                Description = questionToSaveDto.QuestionDescription,
                OptionType = questionToSaveDto.OptionType,
                Marks = questionToSaveDto.Marks,
                Serial = questionToSaveDto.Order,
                IsDeleted = false,
                DateCreated = currentDateTime,
                DateUpdated = null,
                ExamId = questionToSaveDto.ExamId
            };

            foreach (var item in optionsToSaveDto) {
                var option = new Option {
                    Id = Guid.NewGuid(),
                    Description = item.OptionText,
                    IsDeleted = false,
                    DateCreated = currentDateTime,
                    DateUpdated = null
                };

                optionsToSave.Add(option);

                var qoToSave = new QuestionOption {
                    QuestionId = questionGuid,
                    OptionId = option.Id,
                    Order = item.SerialNo,
                    IsDeleted = false,
                    IsCorrectAnswer = item.IsCorrectAnswer,
                    DateCreated = currentDateTime,
                    DateUpdated = null,
                    Question = questionToSave,
                    Option = option
                };

                questionAndOptionsToSave.Add(qoToSave);
            }

            bool result = _unitOfWork.QuestionOptions.IsQuestionAnswerSaved(questionAndOptionsToSave);

            // END

            return result;
        }
    }
}
