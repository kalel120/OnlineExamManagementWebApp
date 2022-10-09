using System.Collections.Generic;
using System.Web.Http;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.DTOs;

namespace OnlineExamManagementWebApp.Controllers.Api {
    [Authorize(Roles = "Admin")]
    public class ExamController : ApiController {
        private readonly ExamManager _examManager;

        public ExamController() {
            _examManager = new ExamManager();
        }

        // Exam/Index
        [HttpGet]
        public IHttpActionResult GetAllExamsForIndex() {
            ICollection<ExamDetailsDto> exams = _examManager.GetAllExamsForIndex();
            return Ok(exams);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id) {
            if (id <= 0) {
                return BadRequest("Invalid request");
            }

            if (_examManager.DeleteExamById(id)) {
                return Ok(true);
            }

            return NotFound();
        }
    }
}
