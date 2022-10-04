using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
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

        public bool IsQuestionAnswerSaved(QuestionToSaveDto questionToSaveDto,
            ICollection<OptionToSaveDto> optionsToSaveDto) {
            DateTime currentDateTime = DateTime.Now;
            ICollection<Option> optionsToSave = new List<Option>();
            ICollection<QuestionOption> questionAndOptionsToSave = new List<QuestionOption>();

            var questionToSave = MapQuestionWithDto(questionToSaveDto, currentDateTime);

            // Adding the question and it's options to bridging table list. Saving this list will auto save question and options in DB
            foreach (var item in optionsToSaveDto) {
                var option = MapOptionWithDto(item, currentDateTime);
                optionsToSave.Add(option);
                questionAndOptionsToSave.Add(MapQuestionOptionBridgeTable(questionToSave, option, item.SerialNo,
                    item.IsCorrectAnswer, currentDateTime, questionToSave.ExamId));
            }

            return _unitOfWork.QuestionOptions.IsQuestionAnswerSaved(questionAndOptionsToSave);
        }

        private QuestionOption MapQuestionOptionBridgeTable(Question question, Option option, int serialNo,
            bool isCorrectAnswer, DateTime currentDateTime, int examId) {
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
            if (dto.OptionType == "Single Answer") {
                IEnumerable<QuestionOption> questionOptionsToUpdate =
                    _unitOfWork.QuestionOptions.GetRowsByQuestionAndExamId(dto.QuestionId, dto.ExamId);

                if (questionOptionsToUpdate == null || !questionOptionsToUpdate.Any()) {
                    return false;
                }

                // As there should be only one correct ans for single ans type, on update -> other options ans will be false by default
                foreach (var item in questionOptionsToUpdate) {
                    if (item.OptionId != dto.OptionId) {
                        item.IsCorrectAnswer = false;
                    }
                    else {
                        item.IsCorrectAnswer = dto.IsCorrectAnswer;
                        //_unitOfWork.QuestionOptions.ModifyEntityState(item);
                    }
                }
            }

            if (dto.OptionType == "Multiple Answer") {
                QuestionOption questionOptionToUpdate =
                    _unitOfWork.QuestionOptions.GetRowForSingleOptionById(dto.OptionId);

                if (questionOptionToUpdate == null) {
                    return false;
                }

                questionOptionToUpdate.IsCorrectAnswer = dto.IsCorrectAnswer;
            }

            return _unitOfWork.Complete();
        }

        public bool IsSingleQuestionUpdated(QuestionToUpdateDto dto) {
            try {
                Question questionToUpdate = _unitOfWork.Questions.GetSingleQuestionById(dto.QuestionId);

                if (questionToUpdate == null) {
                    return false;
                }

                questionToUpdate = MapAndReturnSingleQuestionToDto(questionToUpdate, dto);

                return _unitOfWork.Complete();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private Question MapAndReturnSingleQuestionToDto(Question questionToUpdate, QuestionToUpdateDto dto) {
            questionToUpdate.Description = dto.Description;
            questionToUpdate.OptionType = dto.OptionType;
            questionToUpdate.Marks = dto.Marks;
            questionToUpdate.Serial = dto.Serial;
            questionToUpdate.DateUpdated = DateTime.Now;

            return questionToUpdate;
        }

        public bool IsAssignedQuestionRemoved(QuOpBridgeTblRemoveQuestionDto dto) {
            try {
                DateTime currentDateTime = DateTime.Now;
                IEnumerable<QuestionOption> questionOptionsToUpdate =
                    _unitOfWork.QuestionOptions.GetRowsByQuestionAndExamId(dto.QuestionId, dto.ExamId);

                if (questionOptionsToUpdate == null || !questionOptionsToUpdate.Any()) {
                    return false;
                }

                foreach (var item in questionOptionsToUpdate) {
                    item.IsDeleted = dto.IsDeleted;
                    item.DateUpdated = currentDateTime;
                }

                return _unitOfWork.Complete();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool IsQuestionsReOrdered(IEnumerable<QuestionToUpdateDto> questionsToUpdateDto) {
            try {
                // A cast to System.Data.SqlTypes.SqlGuid is needed to perform
                // the same in-memory linq-sorting-result in comparison to SQL-Server sorting of UNIQUE ID

                var dtoList = questionsToUpdateDto.OrderBy(x => new SqlGuid(x.QuestionId)).ToList();
                var examId = dtoList.Select(q => q.ExamId).FirstOrDefault();


                List<Question> questions = _unitOfWork.QuestionOptions
                    .GetRowsByExamIdWithQuestion(examId)
                    .Select(qo => qo.Question)
                    .Distinct()
                    .OrderBy(q => new SqlGuid(q.Id))
                    .ToList();

                if (!questions.Any()) {
                    return false;
                }

                for (int index = 0; index < questions.Count; index++) {
                    questions[index] = MapAndReturnSingleQuestionToDto(questions[index], dtoList[index]);
                }
                return _unitOfWork.Complete();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}