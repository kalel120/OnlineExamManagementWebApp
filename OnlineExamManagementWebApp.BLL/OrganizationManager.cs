using System.Collections.Generic;
using System.Web.Mvc;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {
    public class OrganizationManager {
        private readonly UnitOfWork _unitOfWork;

        public OrganizationManager() {
            _unitOfWork = new UnitOfWork();
        }

        public List<SelectListItem> GetAllOrganizations() {
            var organizations = _unitOfWork.Organizations.GetAllOrganizations();
            organizations.Insert(0, new SelectListItem { Value = "", Text = "--SELECT ORGANIZATION--" });
            return organizations;
        }

        public Organization GetOrganizationById(int organizationId) {
            return _unitOfWork.Organizations.GetOrganizationById(organizationId);
        }
    }
}
