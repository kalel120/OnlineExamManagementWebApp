using System.Web.Mvc;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.ViewModels;

namespace OnlineExamManagementWebApp.Controllers {
    public class CourseController : Controller {
        private readonly CourseManager _courseManager = new CourseManager();

        public ActionResult Entry() {
            var courseEntryViewModel = new CourseEntryViewModel {
                Organizations = _courseManager.GetAllOrganizations(),
                Tags = new SelectList(_courseManager.GetAllTags())
            };

            return View(courseEntryViewModel);
        }
    }
}