using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineExamManagementWebApp.DTOs;
using OnlineExamManagementWebApp.ViewModels;


namespace OnlineExamManagementWebApp.Controllers {
    public class QuestionAnswerController : Controller {

        public ActionResult Entry(int examId) {
            var quesAnsViewModel = new QuestionAnswerViewModel();
            // Get Organization, Course, Exam Code info. from BLL

            // Get List of Questions from BLL
            // Display Them on entry page
            return View();
        }
    }
}