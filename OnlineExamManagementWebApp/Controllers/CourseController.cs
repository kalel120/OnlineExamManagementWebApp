using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.DTOs;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.ViewModels;

namespace OnlineExamManagementWebApp.Controllers {
    public class CourseController : Controller {
        private readonly CourseManager _courseManager;
        private readonly TrainerManager _trainerManager;
        private readonly CourseTrainerManager _courseTrainerManager;
        private readonly ExamManager _examManager;
        private readonly OrganizationManager _orgManager;

        public CourseController() {
            _courseManager = new CourseManager();
            _trainerManager = new TrainerManager();
            _courseTrainerManager = new CourseTrainerManager();
            _examManager = new ExamManager();
            _orgManager = new OrganizationManager();
        }

        #region course entry page
        public ActionResult Entry() {
            return View(GetCourseEntryViewModel());
        }

        public JsonResult GetAllTags() {
            var tags = _courseManager.GetAllTags().Select(t => new { id = t.Id, text = t.Name });
            return Json(tags, JsonRequestBehavior.AllowGet);
        }

        private CourseEntryViewModel GetCourseEntryViewModel() {
            var entryViewModel = new CourseEntryViewModel {
                Organizations = _orgManager.GetAllSelectListOrganizations("")
            };
            return entryViewModel;
        }

        [HttpPost]
        public ActionResult Entry(CourseEntryViewModel courseEntryVm) {
            if (ModelState.IsValid) {
                var course = Mapper.Map<Course>(courseEntryVm);

                /*
                 * tag management
                 */
                var existingTags = new List<int>();
                var newTags = new List<string>();

                foreach (var item in courseEntryVm.SelectedTags) {
                    if (int.TryParse(item, out var tagId)) {
                        existingTags.Add(tagId);
                    }
                    else {
                        newTags.Add(item);
                    }
                }

                course.TagIds = _courseManager.GetTags(existingTags, newTags).ToList();

                // end
                //course.Tags = _courseManager.GetSelectedTags(courseEntryVm.SelectedTags).ToList();

                if (!_courseManager.IsCourseSaved(course))
                    return RedirectToAction("Error");

                return RedirectToAction("Edit", new { id = course.Id });
            }

            return RedirectToAction("Error");
        }

        public ActionResult Error() {
            return View("Error");
        }
        #endregion

        #region course edit page
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course = _courseManager.GetCourseWithActiveExamsById(id);
            course.Organization = _orgManager.GetOrganizationById(course.OrganizationId);

            var createExamVm = new CreateExamViewModel {
                Course = course,
                OrganizationName = course.Organization.Name
            };

            var courseEditVm = Mapper.Map<CourseEditViewModel>(course);
            courseEditVm.CreateExamVm = createExamVm;

            ViewBag.Course = course;
            return View(courseEditVm);
        }
        #endregion

        #region DisplayAllCourses
        public ActionResult List() {
            ICollection<CourseListViewModel> courseListVm = _courseManager.GetAllActiveCoursesWithOrganization()
                .Select(dto => Mapper.Map<CourseListViewModel>(dto))
                .ToList();

            return View(courseListVm);
        }
        #endregion

        #region assignTrainer tab
        public JsonResult GetModifiedListOfTrainers(string searchTerm, int id) {
            var trainerList = _trainerManager.GetTrainersByOrgId(id);
            if (searchTerm != null) {
                trainerList = trainerList.Where(t => t.Name.Contains(searchTerm)).ToList();
            }

            var modifiedList = trainerList.Select(x => new { id = x.Id, text = x.Name }).ToList();
            modifiedList.Insert(0, new { id = 0, text = "--Select Trainer--" });

            return Json(modifiedList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTrainersByCourse(int id) {
            var courseTrainerList = _courseTrainerManager.GetCourseTrainersByCourseId(id);
            var trainers = new List<CourseTrainerDto>();

            foreach (var item in courseTrainerList) {
                var dto = Mapper.Map<CourseTrainerDto>(item);
                trainers.Add(dto);
            }

            return Json(trainers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AssignTrainer(List<CourseTrainerDto> dtos) {
            var courseTrainerList = new List<CourseTrainer>();

            foreach (var item in dtos) {
                courseTrainerList.Add(Mapper.Map<CourseTrainer>(item));
            }

            var result = _courseTrainerManager.AssignTrainerOfACourse(courseTrainerList);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnassignTrainer(CourseTrainerDto dto) {
            var result = _courseTrainerManager.RemoveTrainerAssignment(Mapper.Map<CourseTrainer>(dto));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateLeadTrainer(CourseTrainerDto dto) {
            var result = _courseTrainerManager.UpdateLeadTrainerStatus(Mapper.Map<CourseTrainer>(dto));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region create Exam tab
        private List<Exam> GetExamsUsingMapper(List<ExamDto> examDtos) {
            var exams = new List<Exam>();

            foreach (var item in examDtos) {
                exams.Add(Mapper.Map<Exam>(item));
            }

            return exams;
        }

        public JsonResult SaveAllExams(List<ExamDto> examDtos) {
            var exams = GetExamsUsingMapper(examDtos);
            var result = _examManager.SaveExams(exams);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsNeedReSequancing(List<ExamDto> examDtos) {
            var exams = GetExamsUsingMapper(examDtos);
            var result = _examManager.IsNeedReSequancing(exams);
            return Json(result);
        }

        public JsonResult ReSequanceSerial(List<ExamDto> examDtos) {
            var exams = GetExamsUsingMapper(examDtos);
            var result = _examManager.ReSequanceSerial(exams);
            return Json(result);
        }

        public JsonResult IsExamExists(int courseId, string examCode) {
            var result = _examManager.IsExamExists(courseId, examCode);
            return Json(result);
        }

        public JsonResult RemoveExamByCode(ExamDto dto) {
            var result = _examManager.RemoveExamByCode(dto.Code, dto.CourseId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateExamByCode(string existingCode, ExamDto dto) {
            var exam = Mapper.Map<Exam>(dto);
            var result = _examManager.UpdateExamByCode(existingCode, exam);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region course search page

        private SearchCourseViewModel GetInitialCourseSearchVm(string selectedOrgId, string selectedTrainerId) {
            var searchViewModel = new SearchCourseViewModel {
                Organizations = _orgManager.GetAllSelectListOrganizations(selectedOrgId),
                Trainers = new SelectList(_trainerManager.GetEmptySelectList(), "Value", "Text", selectedTrainerId).ToList()
            };

            return searchViewModel;
        }

        public ActionResult Search() {
            var searchViewModel = GetInitialCourseSearchVm("", "");
            return View(searchViewModel);
        }

        [HttpPost]
        public ActionResult Search(SearchCourseViewModel viewModel) {
            SearchCourseDto searchParams = Mapper.Map<SearchCourseDto>(viewModel);
            var selectedOrgId = Request.Form["OrganizationId"];
            var selectedTrainerId = Request.Form["TrainerId"];

            if (selectedOrgId == "" && selectedTrainerId == ""
                && string.IsNullOrEmpty(searchParams.Name)
                && string.IsNullOrEmpty(searchParams.Code)
                && searchParams.DurationFrom == null
                && searchParams.DurationTo == null) {

                viewModel = GetInitialCourseSearchVm(selectedOrgId, selectedTrainerId);

                viewModel.Courses = _courseManager.SearchCoursesByParams(searchParams).ToList();
            }
            else if (selectedOrgId == "" || selectedTrainerId == "") {
                viewModel = GetInitialCourseSearchVm(selectedOrgId, selectedTrainerId);
                viewModel.Courses = _courseManager.SearchCoursesByParams(searchParams).ToList();
            }
            else {
                viewModel.Organizations = _orgManager.GetAllSelectListOrganizations(selectedOrgId);
                viewModel.Trainers = new SelectList(_trainerManager.GetSelectListTrainersByOrgId(selectedOrgId), "Value", "Text", selectedTrainerId).ToList();

                viewModel.Courses = _courseManager.SearchCoursesByParams(searchParams).ToList();
            }

            return View(viewModel);
        }

        public JsonResult GetTrainersByOrganization(int id) {
            var trainers = _trainerManager.GetTrainersByOrgId(id);
            return Json(trainers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCourse(CourseEditViewModel editInfo) {
            var updateableCourse = Mapper.Map<Course>(editInfo);
            bool isCourseUpdated = _courseManager.IsCourseUpdated(updateableCourse);
            bool isCourseTrainerUpdated = _courseTrainerManager.IsLeadTrainerStatusUpdatedById(editInfo.LeadTrainerId, editInfo.Id);

            return Json(isCourseUpdated && isCourseTrainerUpdated, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteCourse(int courseId) {
            bool isCourseDeleted = _courseManager.IsCourseDeleted(courseId);
            return Json(isCourseDeleted, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}