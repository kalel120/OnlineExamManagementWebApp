using System.Collections.Generic;

namespace OnlineExamManagementWebApp.ViewModels {
    public class QuestionAnswerEntryViewModel {
        public int ExamId { get; set; }
        public int Order { get; set; }
        public int Marks { get; set; }
        public string QuestionDescription { get; set; }
        public string OptionType { get; set; }
        public ICollection<OptionEntryViewModel> Options { get; set; }
    }
}