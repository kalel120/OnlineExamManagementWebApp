using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.DTOs;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.ViewModels;

namespace OnlineExamManagementWebApp.Controllers {
    public class OrganizationController : Controller {

        private readonly OrganizationManager _orgManager;

        public OrganizationController() {
            _orgManager = new OrganizationManager();
        }

        public ActionResult Index() {
            return View(_orgManager.GetAllOrganizations());
        }

        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var organization = _orgManager.GetOrganizationWithCoursesAndTrainers(Convert.ToInt32(id));
            if (organization == null) {
                return HttpNotFound();
            }
            return View(organization);
        }

        #region CreateNewOrganization

        [HttpGet]
        public ActionResult Entry() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Entry(OrgEntryViewModel viewModel) {
            if (ModelState.IsValid) {
                var org = Mapper.Map<Organization>(viewModel);

                if (Request.Files[0] != null) {
                    using (MemoryStream memory = new MemoryStream()) {
                        Request.Files[0].InputStream.CopyTo(memory);
                        org.Logo = memory.ToArray();
                    }
                }

                if (_orgManager.IsOrganizationSaved(org)) {
                    return RedirectToAction("Index");
                }
            }

            return View("Error");
        }
        #endregion

        #region SearchOrganization

        [HttpGet]
        public ActionResult Search() {
            return View(new SearchOrgViewModel());
        }

        [HttpGet]
        public ActionResult SearchResult(SearchOrgDto dto) {
            SearchOrgViewModel orgsViewModel = new SearchOrgViewModel();

            if (IsEveryPropertyNullOrEmpty(dto)) {
                orgsViewModel.Organizations = _orgManager.GetAllOrganizations().ToList();
            }
            else {
                orgsViewModel.Organizations = _orgManager.GetOrganizationsByParams(dto).ToList();
            }

            ViewData["Orgs"] = orgsViewModel.Organizations;

            return View("Search", orgsViewModel);
        }

        private static bool IsEveryPropertyNullOrEmpty(object myObject) {
            return myObject.GetType().GetProperties()
                .Where(pi => pi.PropertyType == typeof(string))
                .Select(pi => (string)pi.GetValue(myObject))
                .All(string.IsNullOrEmpty);
        }

        #endregion

        public ActionResult GetOrgLogo(int orgId) {
            var imageData = _orgManager.GetLogoByOrgId(orgId);
            return File(imageData, "image/jpg");
        }

        public JsonResult Delete(int orgId) {
            return Json(_orgManager.IsOrganizationDeleted(orgId));
        }

        public JsonResult UpdateOrganization(UpdateOrgDto dto, int orgId) {
            if (_orgManager.IsOrganizationUpdated(dto, orgId)) {
                return Json(true);
            }
            return Json(false);
        }
    }
}