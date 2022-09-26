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
            return _unitOfWork.QuestionOptions.GetQuestionsByExamId(examId);
        }

        public bool IsQuestionAnswerSaved(QuestionToSaveDto questionToSaveDto, ICollection<OptionToSaveDto> optionsToSaveDto) {
            DateTime currentDateTime = DateTime.Now;
            ICollection<Option> optionsToSave = new List<Option>();
            ICollection<QuestionOption> questionAndOptionsToSave = new List<QuestionOption>();

            var questionToSave = MapQuestionWithDto(questionToSaveDto, currentDateTime);

            // Adding the question and it's options to bridging table list. Saving this list will auto save question and options in DB
            foreach (var item in optionsToSaveDto) {
                var option = MapOptionWithDto(item, currentDateTime);
                optionsToSave.Add(option);
                questionAndOptionsToSave.Add(MapQuestionOptionBridgeTable(questionToSave, option, item.SerialNo, item.IsCorrectAnswer, currentDateTime, questionToSave.ExamId));
            }

            return _unitOfWork.QuestionOptions.IsQuestionAnswerSaved(questionAndOptionsToSave);
        }

        private QuestionOption MapQuestionOptionBridgeTable(Question question, Option option, int serialNo, bool isCorrectAnswer, DateTime currentDateTime, int examId) {
            return new QuestionOption {
                QuestionId = question.Id,
                OptionId = option.Id,
                Order = serialNo,
                IsDeleted = false,
                IsCorrectAnswer = isCorrectAnswer,
                DateCreated = currentDateTime,
                DateUpdated = null,
                Question = question,
                Option = option,
                ExamId = examId
            };

        }

        private Question MapQuestionWithDto(QuestionToSaveDto dto, DateTime currentDateTime) {
            return new Question {
                Id = Guid.NewGuid(),
                Description = dto.QuestionDescription,
                OptionType = dto.OptionType,
                Marks = dto.Marks,
                Serial = dto.Order,
                IsDeleted = false,
                DateCreated = currentDateTime,
                DateUpdated = null,
                ExamId = dto.ExamId
            };
        }

        private Option MapOptionWithDto(OptionToSaveDto dto, DateTime currentDateTime) {
            return new Option {
                Id = Guid.NewGuid(),
                Description = dto.OptionText,
                IsDeleted = false,
                DateCreated = currentDateTime,
                DateUpdated = null
            };
        }

        public ICollection<OptionDto> GetOptionsByQuestionId(Guid questionId) {
            return _unitOfWork.QuestionOptions.GetOptionsByQuestionId(questionId);
        }

        public bool IsOptionRemoved(OptionToUpdate dToUpdate) {
            return _unitOfWork.QuestionOptions.IsOptionRemoved(dToUpdate.OptionId, dToUpdate.ExamId);
        }

        public bool IsOptionReordered(ICollection<OptionToUpdate> dto, int examId, Guid questionId) {
            return _unitOfWork.QuestionOptions.IsOptionReordered(dto, examId, questionId);
        }

        public bool IsSingleOptionSaved(SingleOptionToSave dtoOptionToSave) {
            return _unitOfWork.QuestionOptions.IsSingleOptionSaved(dtoOptionToSave);
        }

        public bool IsCorrectAnsOfOptionUpdated(OptionToUpdate dto) {
            return _unitOfWork.QuestionOptions.IsCorrectAnsOfOptionUpdated(dto);
        }
    }
}
