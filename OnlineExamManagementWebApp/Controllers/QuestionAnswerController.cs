using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.DTOs;
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
            var viewModel = new QuestionAnswerViewModel();

            // Get Organization, Course, Exam Code info. from BLL
            var examDetails = _examManager.GetExamDetailsById(examId);
            viewModel.CourseName = examDetails.CourseName;
            viewModel.OrganizationName = examDetails.OrganizationName;
            viewModel.ExamCode = examDetails.Code;

            // Get List of Questions from BLL
            viewModel.Questions = _qoManager.GetQuestionsByExamId(examId);

            // Display Them on entry page
            return View(viewModel);
        }

        public JsonResult GetQuestionsByExamId(int id) {
            ICollection<QuestionsDto> questions = _qoManager.GetQuestionsByExamId(id);
            return Json(questions, JsonRequestBehavior.AllowGet);
        }
    }

}