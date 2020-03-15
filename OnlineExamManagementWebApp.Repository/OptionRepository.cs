using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineExamManagementWebApp.DatabaseContext;

namespace OnlineExamManagementWebApp.Repository {
    public class OptionRepository {
        private readonly ApplicationDbContext _dbContext;
        public OptionRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }
    }
}
