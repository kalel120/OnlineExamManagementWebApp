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

        public List<SelectListItem> GetAllOrganizations() {
            return _dbContext.Organizations
                .Where(o => o.IsDeleted == false)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();
        }
    }
}