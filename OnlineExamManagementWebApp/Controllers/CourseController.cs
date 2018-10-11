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
        private readonly CourseManager _courseManager = new CourseManager();
        private readonly TrainerManager _trainerManager = new TrainerManager();
        private readonly CourseTrainerManager _courseTrainerManager = new CourseTrainerManager();
        private readonly ExamManager _examManager = new ExamManager();

        #region Course Entry Page
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
        public ActionResult Entry(CourseEntryViewModel courseEntryVm) {
            if (ModelState.IsValid) {
                var course = Mapper.Map<Course>(courseEntryVm);
                course.Tags = _courseManager.GetReleventTags(courseEntryVm.SelectedTags);

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

                     
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course = _courseManager.GetCourseWithActiveExamsById(id);
            course.Organization = _courseManager.GetOrganizationById(course.OrganizationId);

            var createExamVm = new CreateExamViewModel {
                Course = course,
                OrganizationName = course.Organization.Name
            };

            var courseEditVm = Mapper.Map<CourseEditViewModel>(course);
            courseEditVm.CreateExamVm = createExamVm;

            ViewBag.Course = course;
            return View(courseEditVm);
        }

        #region AssignTrainer tab page 
        public JsonResult GetTrainersByOrganization(string searchTerm, int orgId) {
            var trainerList = _trainerManager.GetTrainersByOrgId(orgId);
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

        #region Create Exam tab 
        public JsonResult SaveAllExams(List<CreateExamViewModel> createExamsVmList) {
            var exams = new List<Exam>();

            foreach (var item in createExamsVmList) {
                exams.Add( Mapper.Map<Exam>(item));
            }

            var result = _examManager.SaveExams(exams);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReSequanceSerial(List<ExamDto> examDtos) {
            var exams = new List<Exam>();

            foreach (var item in examDtos) {
                exams.Add(Mapper.Map<Exam>(item));
            }

            var result = _examManager.ReSequanceSerial(exams);
            return Json(result);
        }

        public JsonResult IsExamExists(int courseId, string examCode) {
            bool result = _examManager.IsExamExists(courseId, examCode);
            return Json(result);
        }

        public JsonResult RemoveExamByCode(ExamDto dto) {
            bool result = _examManager.RemoveExamByCode(dto.Code,dto.CourseId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateExamByCode(string existingCode, ExamDto dto) {
            var exam = Mapper.Map<Exam>(dto);

            bool result = _examManager.UpdateExamByCode(existingCode,exam);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}