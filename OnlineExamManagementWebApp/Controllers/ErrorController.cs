using System.Web.Mvc;

namespace OnlineExamManagementWebApp.Controllers {
    public class ErrorController : Controller {

        public ActionResult Error404() {
            return View("Error_404");
        }
    }
}