using System.Collections.Generic;
using System.Web.Http;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Controllers.Api {
    public class ExamController : ApiController {
        private readonly ExamManager _examManager;

        public ExamController() {
            _examManager = new ExamManager();
        }

        // Exam/Index
        [HttpGet]
        public IHttpActionResult Index() {
            ICollection<Exam> exams = _examManager.GetAllExams();
            return Ok(exams);
        }
    }
}
