using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineExamManagementWebApp.ViewModels {
    public class CourseEntryViewModel {
        [Required]
        [Display(Name = "Organization")]
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

        // Not needed
        //public IEnumerable<SelectListItem> Tags { get; set; }

        [Required]
        [Display(Name = "Tags")]
        public List<string> SelectedTags { get; set; }
    }
}