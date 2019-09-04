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
            var organizations = _unitOfWork.Organizations.GetAllSelectListOrganizations();
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
            _unitOfWork.Organizations.Save(organization);
            return _unitOfWork.Complete();
        }

        public byte[] GetLogoByOrgId(int orgId) {
            return _unitOfWork.Organizations.GetLogoByOrgId(orgId);
        }

        public bool IsOrganizationDeleted(int orgId) {
            _unitOfWork.Organizations.Delete(orgId);
            return _unitOfWork.Complete();
        }
    }
}
