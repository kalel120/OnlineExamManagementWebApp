namespace OnlineExamManagementWebApp.DTOs {
    public class ExamDto {
        public string Code { get; set; }

        public string Type { get; set; }

        public string Topic { get; set; }

        public double? FullMarks { get; set; }

        public int Duration { get; set; }

        public int? DurationHour { get; set; }

        public int? DurationMin { get; set; }

        public int SerialNo { get; set; }

        public int CourseId { get; set; }

        public bool IsDeleted { get; set; }
    }
}