using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineExamManagementWebApp.ViewModels {
    public class CourseEntryViewModel {
        [Display(Name = "Organization")]
        [Required]
        public int OrganizationId { get; set; }

        public List<SelectListItem> Organizations { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public double Credit { get; set; }

        [Required]
        public string Outline { get; set; }

        public IEnumerable<SelectListItem> Tags { get; set; }

        [Display(Name = "Tags")]
        public List<string> SelectedTags { get; set; }
    }
}