using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OnlineExamManagementWebApp.DTOs;
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

        public Organization GetOrganizationWithCoursesAndTrainers(int orgId) {
            var org = GetOrganizationById(orgId);
            org.Courses = _unitOfWork.Courses.GetCoursesByOrgId(orgId).ToList();
            org.Trainers = _unitOfWork.Trainers.GetTrainersByOrgId(orgId);
            return org;
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

        public ICollection<Organization> GetOrganizationsByParams(SearchOrgDto dto) {
            ICollection<Organization> orgs = null;

            if (dto.Name != null) {
                orgs = _unitOfWork.Organizations.GetOrgsContainsName(dto.Name);
            }

            if (dto.Code != null) {
                orgs = _unitOfWork.Organizations.GetOrgsContainsCode(dto.Code);
            }

            if (dto.Contact != null) {
                orgs = _unitOfWork.Organizations.GetOrgsContainsContact(dto.Contact);
            }

            if (dto.Address != null) {
                orgs = _unitOfWork.Organizations.GetOrgsContainsAddress(dto.Address);
            }

            return orgs;
        }

        public bool IsOrganizationUpdated(UpdateOrgDto dto, int orgId) {
            var orgToBeUpdated = _unitOfWork.Organizations.GetOrganizationById(orgId);
            orgToBeUpdated.Name = dto.Name;
            orgToBeUpdated.Code = dto.Code;
            orgToBeUpdated.Address = dto.Address;
            orgToBeUpdated.Contact = dto.Contact;

            return _unitOfWork.Complete();
        }
    }
}
