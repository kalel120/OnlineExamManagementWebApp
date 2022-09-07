namespace OnlineExamManagementWebApp.DTOs.QuestionOption {
    class QuestionToSaveDto {
        public int Order { get; set; }
        public int Marks { get; set; }
        public string QuestionDescription { get; set; }
        public string OptionType { get; set; }

        public int ExamId { get; set; }
    }
}
