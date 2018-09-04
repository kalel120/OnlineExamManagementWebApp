using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineExamManagementWebApp.ViewModels {
    public class CourseEntryViewModel {
        [Display(Name = "Organization")]
        public int OrganizationId { get; set; }

        public List<SelectListItem> Organizations { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int Duration { get; set; }

        public double Credit { get; set; }

        public string Outline { get; set; }

        public IEnumerable<SelectListItem> Tags { get; set; }

        [Display(Name = "Tags")]
        public List<string> SelectedTags { get; set; }
    }
}