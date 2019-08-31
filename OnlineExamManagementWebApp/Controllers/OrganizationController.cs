using System.Data.Entity;
using System.Net;
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
        public ActionResult Create() {
            return View();
        }

        // POST: Organization/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Code,Address,Contact,About,Logo,IsDeleted")] Organization organization) {
            if (ModelState.IsValid) {
                db.Organizations.Add(organization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(organization);
        }

        // GET: Organization/Edit/5
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organizations.Find(id);
            if (organization == null) {
                return HttpNotFound();
            }
            return View(organization);
        }

        // POST: Organization/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Code,Address,Contact,About,Logo,IsDeleted")] Organization organization) {
            if (ModelState.IsValid) {
                db.Entry(organization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organization);
        }

        // GET: Organization/Delete/5
        public ActionResult Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organizations.Find(id);
            if (organization == null) {
                return HttpNotFound();
            }
            return View(organization);
        }

        // POST: Organization/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Organization organization = db.Organizations.Find(id);
            db.Organizations.Remove(organization);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
