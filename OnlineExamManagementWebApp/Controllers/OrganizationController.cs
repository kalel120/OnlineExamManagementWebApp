using System.Web.Mvc;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Controllers {
    public class OrganizationController : Controller {

        private readonly OrganizationManager _orgManager;
        public OrganizationController() {
            _orgManager = new OrganizationManager();
        }

        // GET: Organization
        public ActionResult Index() {
            return View(_orgManager.GetAllOrganizations());
        }

        // GET: Organization/Details/5
        public ActionResult Details(int id) {
            //if (id == null) {
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            var organization = _orgManager.GetOrganizationById(id);
            if (organization == null) {
                return HttpNotFound();
            }
            return View(organization);
        }

        // GET: Organization/Create
        public ActionResult Entry() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Entry([Bind(Include = "Id,Name,Code,Address,Contact,About,Logo,IsDeleted")] Organization organization) {
            if (ModelState.IsValid) {
                if (_orgManager.IsOrganizationSaved(organization)) {
                    return RedirectToAction("Index");
                }
            }

            return View(organization);
        }
    }
}
