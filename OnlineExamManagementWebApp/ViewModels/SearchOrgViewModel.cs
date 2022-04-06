using System.Collections.Generic;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.ViewModels {
    public class SearchOrgViewModel {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Address { get; set; }

        public string Contact { get; set; }

        public List<Organization> Organizations { get; set; }
    }
}