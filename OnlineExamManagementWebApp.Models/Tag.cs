using System.Collections.Generic;

namespace OnlineExamManagementWebApp.Models {
    public class Tag {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Course> Courses { get; set; }
    }
}
