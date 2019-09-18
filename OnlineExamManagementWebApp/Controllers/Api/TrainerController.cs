using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.DTOs;

namespace OnlineExamManagementWebApp.Controllers.Api {
    public class TrainerController : ApiController {
        private readonly TrainerManager _trainerManager;

        public TrainerController() {
            _trainerManager = new TrainerManager();
        }

        [HttpDelete]
        public HttpResponseMessage DeleteTrainer([FromBody] DeleteTrainerDto dto) {
            try {
                if (!_trainerManager.IsTrainerDeleted(dto.TrainerId)) {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trainer not found!");
                }

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception exception) {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception);
            }
        }
    }
}
