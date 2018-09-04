using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {

    public class CourseManager {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public List<SelectListItem> GetAllOrganizations() {
            var organizations = _unitOfWork._courseRepository.GetAllOrganizations();
            organizations.Insert(0, new SelectListItem() { Value = "", Text = "--Select Organization--" });
            return organizations;
        }
    }
}
