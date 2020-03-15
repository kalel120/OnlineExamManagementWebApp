using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamManagementWebApp.Controllers {
    public class QuestionAnswerController : Controller {

        // Get
        public ActionResult QuestionAnswer(int examId) {
            return View();
        }
    }
}