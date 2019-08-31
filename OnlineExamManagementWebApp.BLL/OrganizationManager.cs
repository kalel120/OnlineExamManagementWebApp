using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {
    public class OrganizationManager {
        private readonly UnitOfWork _unitOfWork;

        public OrganizationManager() {
            _unitOfWork = new UnitOfWork();
        }

        public List<SelectListItem> GetAllSelectListOrganizations(string selectedOrgId) {
            var organizations = _unitOfWork.Organizations.GetAllSelectListOrgnizations();
            organizations.Insert(0, new SelectListItem { Value = "", Text = "--SELECT ORGANIZATION--" });
            return new SelectList(organizations, "Value", "Text", selectedOrgId).ToList();
        }

        public Organization GetOrganizationById(int organizationId) {
            return _unitOfWork.Organizations.GetOrganizationById(organizationId);
        }

        public ICollection<Organization> GetAllOrganizations() {
            return _unitOfWork.Organizations.GetAllOrganizations();
        }

        public bool IsOrganizationSaved(Organization organization) {
            return _unitOfWork.Organizations.IsOrganizationSaved(organization);
        }
    }
}
