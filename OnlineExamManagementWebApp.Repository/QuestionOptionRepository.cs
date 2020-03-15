using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineExamManagementWebApp.DatabaseContext;

namespace OnlineExamManagementWebApp.Repository {
    public class QuestionOptionRepository {
        private readonly ApplicationDbContext _dbContext;
        public QuestionOptionRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }
    }
}
