using System.Web.Mvc;

namespace OnlineExamManagementWebApp.Controllers {
    public class ErrorController : Controller {

        public ActionResult Error404() {
            return View("Error_404");
        }

        public ActionResult Error() {
            return View("Error");
        }

        public ActionResult Error500() {
            return View("Error_500");
        }
    }
}