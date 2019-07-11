namespace OnlineExamManagementWebApp.DTOs {
    public class SearchCourseDto {
        public int OrganizationId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int? DurationFrom { get; set; }

        public int? DurationTo { get; set; }

        public int TrainerId { get; set; }
    }
}
