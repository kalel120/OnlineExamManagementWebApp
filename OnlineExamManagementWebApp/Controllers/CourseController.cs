﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.DTOs;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.ViewModels;

namespace OnlineExamManagementWebApp.Controllers {
    public class CourseController : Controller {
        private readonly CourseManager _courseManager = new CourseManager();
        private readonly TrainerManager _trainerManager = new TrainerManager();
        private readonly CourseTrainerManager _courseTrainerManager = new CourseTrainerManager();

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

                return RedirectToAction("Edit", new { id = course.Id });
            }

            return RedirectToAction("Error");
        }

        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course = _courseManager.GetCourseById(id);
            course.Organization = _courseManager.GetOrganizationById(course.OrganizationId);
            
            var createExamVm = new CreateExamViewModel {
                Course = course,
                OrganizationName = course.Organization.Name
            };

            var courseEditVm = new CourseEditViewModel {
                Id = course.Id,
                OrganizationId = course.OrganizationId,
                OrganizationCode = course.Organization.Code,
                Name = course.Name,
                Code = course.Code,
                Duration = course.Duration,
                Credit = course.Credit,
                Outline = course.Outline,
                CreateExamVm = createExamVm
            };

            ViewBag.Course = course;
            return View(courseEditVm);
        }

        public ActionResult Error() {
            return View("Error");
        }
        
        public JsonResult GetTrainersByOrganization(string searchTerm,int orgId) {
            var trainerList = _trainerManager.GetTrainersByOrgId(orgId);
            if (searchTerm != null) {
                trainerList = trainerList.Where(t => t.Name.Contains(searchTerm)).ToList();
            }

            var modifiedList = trainerList.Select(x => new { id = x.Id, text = x.Name }).ToList();            
            modifiedList.Insert(0,new{id=0, text= "--Select Trainer--" });

            return Json(modifiedList, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetTrainersByCourse(int id) {            
            var courseTrainerList = _courseTrainerManager.GetCourseTrainersByCourseId(id);
            var trainers = new List<CourseTrainerDto>();

            foreach (var item in courseTrainerList) {
                var courseTrainerDto = new CourseTrainerDto {
                    CourseId = item.CourseId,
                    TrainerName = item.Trainer.Name,
                    TrainerId = item.TrainerId,
                    IsLead = item.IsLead
                };
                trainers.Add(courseTrainerDto);
            }

            return Json(trainers, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult AssignTrainer(List<CourseTrainerDto> dtos) {
            var courseTrainerList = new List<CourseTrainer>();

            foreach (var item in dtos) {
                courseTrainerList.Add(                
                    new CourseTrainer {
                        CourseId = item.CourseId,
                        TrainerId = item.TrainerId,
                        IsLead = item.IsLead
                    }
                );
            }

            var result = _courseTrainerManager.AssignTrainerOfACourse(courseTrainerList);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnassignTrainer(CourseTrainerDto dto) {
            var removableTrainer = new CourseTrainer {
                CourseId = dto.CourseId,
                TrainerId = dto.TrainerId
            };

            var result = _courseTrainerManager.RemoveTrainerAssignment(removableTrainer);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateLeadTrainer(CourseTrainerDto dto) {
            var updatable = new CourseTrainer {
                CourseId = dto.CourseId,
                TrainerId = dto.TrainerId,
                IsLead = dto.IsLead
            };

            var result = _courseTrainerManager.UpdateLeadTrainerStatus(updatable);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}