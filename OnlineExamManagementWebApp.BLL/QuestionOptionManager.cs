using System.Collections.Generic;
using OnlineExamManagementWebApp.Repository;
using OnlineExamManagementWebApp.DTOs.QuestionOption;

namespace OnlineExamManagementWebApp.BLL {
    public class QuestionOptionManager {
        private readonly UnitOfWork _unitOfWork;

        public QuestionOptionManager() {
            _unitOfWork = new UnitOfWork();
        }

        public ICollection<QuestionsDto> GetQuestionsByExamId(int examId) {
            return _unitOfWork.Questions.GetQuestionsByExamId(examId);
        }

        public bool IsQuestionSaved(QuestionToSaveDto questionToSaveDto, ICollection<OptionToSaveDto> optionsToSaveDto) {
            bool result = true;



            return true;
        }
    }
}
