using System.Collections.Generic;
using System.Web.Mvc;

namespace OnlineExamManagementWebApp.ViewModels {
    public class CourseEntryViewModel {

        public IEnumerable<SelectListItem> Organizations { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int Duration { get; set; }

        public double Credit { get; set; }

        public string Outline { get; set; }

        public IEnumerable<SelectListItem> Tags { get; set; }

        public List<string> SelectedTags { get; set; }
    }
}