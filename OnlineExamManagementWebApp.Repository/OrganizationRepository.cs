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
                .Include(o => o.Courses)
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

        public ICollection<Organization> GetOrgsContainsName(string name) {
            var query = _dbContext.Organizations
                .Include(o => o.Courses)
                .Where(o => o.Name.Contains(name) && o.IsDeleted == false).ToList();
            return query;
        }

        public ICollection<Organization> GetOrgsContainsCode(string code) {
            var query = _dbContext.Organizations
                .Include(o => o.Courses)
                .Where(o => o.Code.Contains(code) && o.IsDeleted == false);
            return query.ToList();
        }

        public ICollection<Organization> GetOrgsContainsContact(string contact) {
            var query = _dbContext.Organizations
                .Include(o => o.Courses)
                .Where(o => o.Contact.Contains(contact) && o.IsDeleted == false);
            return query.ToList();
        }

        public ICollection<Organization> GetOrgsContainsAddress(string address) {
            var query = _dbContext.Organizations
                .Include(o => o.Courses)
                .Where(o => o.Address.Contains(address) && o.IsDeleted == false);
            return query.ToList();
        }
    }
}