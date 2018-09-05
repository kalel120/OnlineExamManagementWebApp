using System.Collections.Generic;
using System.Web.Mvc;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.ViewModels;

namespace OnlineExamManagementWebApp.Controllers {
    public class CourseController : Controller {
        private readonly CourseManager _courseManager = new CourseManager();

        public ActionResult Entry() {
            return View(GetCourseEntryViewModel());
        }

        private CourseEntryViewModel GetCourseEntryViewModel() {
            var courseEntryViewModel = new CourseEntryViewModel {
                Organizations = _courseManager.GetAllOrganizations(),
                Tags = new SelectList(_courseManager.GetAllTags())
            };
            return courseEntryViewModel;
        }

        [HttpPost]
        public ActionResult Entry(CourseEntryViewModel viewModel) {
            if (ModelState.IsValid) {
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
                
                TempData["Course"] = course;
                ModelState.Clear();

                return View();
            }

            return RedirectToAction("Error");
        }

        public ActionResult Information() {

            return View();
        }

        public ActionResult Error() {
            return View("Error");
        }
    }
}