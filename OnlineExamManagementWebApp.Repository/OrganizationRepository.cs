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
            return _dbContext.Organizations.Where(o => o.Id == organizationId).SingleOrDefault();
        }

        public List<SelectListItem> GetAllOrganizations() {
            return _dbContext.Organizations.Select(c => new SelectListItem() { Value = c.Id.ToString(), Text = c.Code })
                .ToList();
        }
    }
}
