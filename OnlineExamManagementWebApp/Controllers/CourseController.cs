using System.Web.Mvc;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.ViewModels;

namespace OnlineExamManagementWebApp.Controllers {
    public class CourseController : Controller {
        private CourseManager _courseManager = new CourseManager();

        public ActionResult Entry() {
            // Load all Organizations from database
            var organizations = _courseManager.GetAllOrganizations();
            var courseEntryViewModel = new CourseEntryViewModel();

            courseEntryViewModel.Organizations = organizations;

            return View(courseEntryViewModel);
        }
    }
}