using System.Collections.Generic;

namespace OnlineExamManagementWebApp.ViewModels {
    public class QuestionAnswerEntryViewModel {
        public int Order { get; set; }
        public int Marks { get; set; }
        public string QuestionDescription { get; set; }
        public string OptionType { get; set; }
        public ICollection<OptionForEntryViewModel> Options { get; set; }
    }

    public class OptionForEntryViewModel {
        public int SerialNo { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrectAnswer { get; set; }
    }
}