namespace OnlineExamManagementWebApp.DTOs {
    public class ExamIndexPageDto {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Type { get; set; }

        public string Topic { get; set; }

        public double FullMarks { get; set; }

        public int Duration { get; set; }

        public string CourseName { get; set; }

        public int CourseId { get; set; }

        public int? SerialNo { get; set; }

        public bool IsDeleted { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationName { get; set; }
    }
}
