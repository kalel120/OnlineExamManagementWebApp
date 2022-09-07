namespace OnlineExamManagementWebApp.DTOs.QuestionOption {
    public class OptionToSaveDto {
        public int SerialNo { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrectAnswer { get; set; }
    }
}