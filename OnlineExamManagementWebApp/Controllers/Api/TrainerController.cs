using System.Web.Http;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.DTOs;

namespace OnlineExamManagementWebApp.Controllers.Api {
    [Authorize(Roles = "Admin")]
    public class TrainerController : ApiController {
        private readonly TrainerManager _trainerManager;

        public TrainerController() {
            _trainerManager = new TrainerManager();
        }

        [HttpDelete]
        public IHttpActionResult DeleteTrainer([FromBody] DeleteTrainerDto dto) {
            if (!_trainerManager.IsTrainerDeleted(dto.TrainerId)) {
                return BadRequest("Invalid!");
            }
            return Ok(true);
        }

        [HttpGet]
        public IHttpActionResult GetTrainersById(int id) {
            var trainers = _trainerManager.GetTrainersByOrgId(id);
            if (trainers.Count == 0) {
                return BadRequest("Invalid");
            }

            return Ok(trainers);
        }
    }
}
