using System.Text;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Repository {
    public class OrganizationRepository {
        private readonly ApplicationDbContext _dbContext;

        public OrganizationRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Organization GetOrganizationById(int organizationId) {
            return _dbContext.Organizations.SingleOrDefault(o => o.Id == organizationId && o.IsDeleted == false);
        }

        public List<SelectListItem> GetAllSelectListOrganizations() {
            return _dbContext.Organizations
                .Where(o => o.IsDeleted == false)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();
        }

        public ICollection<Organization> GetAllOrganizations() {
            return _dbContext.Organizations
                .Where(o => o.IsDeleted == false)
                .ToList();
        }

        public void Save(Organization organization) {
            _dbContext.Organizations.Add(organization);
        }

        public byte[] GetLogoByOrgId(int orgId) {
            var result = _dbContext.Organizations
                .Where(o => o.IsDeleted == false && o.Id == orgId)
                .Select(o => o.Logo).FirstOrDefault();

            if (result == null) {
                result = Encoding.ASCII.GetBytes("");
            }

            return result;
        }

        public void Delete(int orgId) {
            var organization = GetOrganizationById(orgId);
            organization.IsDeleted = true;
            _dbContext.Entry(organization).State = EntityState.Modified;
        }
    }
}