using System.Web;

namespace OnlineExamManagementWebApp.ViewModels {
    public class OrgEntryViewModel {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Address { get; set; }

        public string Contact { get; set; }

        public string About { get; set; }

        public HttpPostedFileBase Logo { get; set; }
    }
}