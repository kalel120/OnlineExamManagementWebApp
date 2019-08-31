using System.Collections.Generic;
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

        public List<SelectListItem> GetAllSelectListOrgnizations() {
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

        public bool IsOrganizationSaved(Organization organization) {
            _dbContext.Organizations.Add(organization);
            return _dbContext.SaveChanges() > 0;
        }
    }
}