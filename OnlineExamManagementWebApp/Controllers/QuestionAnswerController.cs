using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.DTOs.QuestionOption;
using OnlineExamManagementWebApp.ViewModels;


namespace OnlineExamManagementWebApp.Controllers {
    public class QuestionAnswerController : Controller {
        private readonly ExamManager _examManager;
        private readonly QuestionOptionManager _qoManager;

        public QuestionAnswerController() {
            _examManager = new ExamManager();
            _qoManager = new QuestionOptionManager();
        }

        public ActionResult Entry(int examId) {
            if (examId == 0) { return RedirectToAction("Error", "Error"); }

            var viewModel = new QuestionAnswerViewModel();

            // Get Organization, Course, Exam Code info. from BLL
            var examDetails = _examManager.GetExamDetailsById(examId);
            viewModel.CourseName = examDetails.CourseName;
            viewModel.OrganizationName = examDetails.OrganizationName;
            viewModel.ExamCode = examDetails.Code;

            // Display Them on entry page
            return View(viewModel);
        }

        #region GET Requests
        public JsonResult GetQuestionsByExamId(int id) {
            if (id == 0) { return Json(false, JsonRequestBehavior.AllowGet); }

            ICollection<QuestionsDto> questions = _qoManager.GetQuestionsByExamId(id);
            return Json(questions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOptionsByQuestionId(Guid id) {
            if (id == Guid.Empty) { return Json(false, JsonRequestBehavior.AllowGet); }

            ICollection<OptionDto> options = _qoManager.GetOptionsByQuestionId(id);
            return Json(options, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region POST Requests
        [HttpPost]
        public JsonResult Entry(QuestionAnswerEntryViewModel qaEntryViewModel) {
            if (qaEntryViewModel == null) { return Json(false, JsonRequestBehavior.AllowGet); }

            ICollection<OptionToSaveDto> optionsToSaveDto = new List<OptionToSaveDto>();

            foreach (var item in qaEntryViewModel.Options) {
                optionsToSaveDto.Add(Mapper.Map<OptionToSaveDto>(item));
            }

            QuestionToSaveDto questionToSaveDto = Mapper.Map<QuestionToSaveDto>(qaEntryViewModel);

            bool result = _qoManager.IsQuestionAnswerSaved(questionToSaveDto, optionsToSaveDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSingleOption(SingleOptionToSave dtoOptionToSave) {
            if (dtoOptionToSave == null) { return Json(false, JsonRequestBehavior.AllowGet); }

            bool result = _qoManager.IsSingleOptionSaved(dtoOptionToSave);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PUT Requests
        [HttpPut]
        public JsonResult RemoveAnOption(OptionToUpdate dto) {
            if (dto == null) { return Json(false, JsonRequestBehavior.DenyGet); }

            var result = _qoManager.IsOptionRemoved(dto);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPut]
        public JsonResult ReOrderOptionsOnRemove(ICollection<OptionToUpdate> dto, int examId, Guid questionId) {
            if (dto == null || examId == 0 || questionId == Guid.Empty) {
                return Json(false, JsonRequestBehavior.DenyGet);
            }

            var result = _qoManager.IsOptionReordered(dto, examId, questionId);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPut]
        public JsonResult UpdateCorrectAnsOfOption(OptionToUpdate dto) {
            if (dto == null || dto.ExamId == 0 || dto.OptionId == Guid.Empty) {
                return Json(false, JsonRequestBehavior.DenyGet);
            }

            var result = _qoManager.IsCorrectAnsOfOptionUpdated(dto);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPut]
        public JsonResult UpdateQuestion(QuestionToUpdateDto dto) {
            if (dto == null || dto.ExamId == 0 || dto.QuestionId == Guid.Empty) {
                return Json(false, JsonRequestBehavior.DenyGet);
            }

            bool result = _qoManager.IsQuestionUpdated(dto);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPut]
        public JsonResult IsAssignedQuestionRemoved(QuOpBridgeTblRemoveQuestionDto dto) {
            if (dto == null || dto.ExamId == 0 || dto.QuestionId == Guid.Empty) {
                return Json(false, JsonRequestBehavior.DenyGet);
            }

            bool result = _qoManager.IsAssignedQuestionRemoved(dto);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        #endregion
    }

}