using System.Collections.Generic;
using System.Web.Mvc;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.Models;
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

        [HttpPost]
        public ActionResult Entry(CourseEntryViewModel viewModel) {
            var course = new Course {
                OrganizationId = viewModel.OrganizationId,
                Name = viewModel.Name,
                Code = viewModel.Code,
                Duration = viewModel.Duration,
                Credit = viewModel.Credit,
                Outline = viewModel.Outline,
                Tags = _courseManager.GetReleventTags(viewModel.SelectedTags)
            };
            

            if (!_courseManager.IsCourseSaved(course))
                return RedirectToAction("Error");

            ViewBag.Message = "Saved Successfully";
            return RedirectToAction("Entry");
        }

        public ActionResult Error() {
            return View("Error");
        }
    }
}