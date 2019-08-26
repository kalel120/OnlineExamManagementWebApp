namespace OnlineExamManagementWebApp.DTOs {
    public class CourseWithOrgNameDto {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string CourseOutline { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
    }
}
