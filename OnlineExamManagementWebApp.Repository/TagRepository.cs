using System.Collections;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;

namespace OnlineExamManagementWebApp.Repository {
    public class TagRepository {
        private readonly ApplicationDbContext _dbContext;

        public TagRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public IEnumerable GetAllTagNames() {
            return _dbContext.Tags.Select(x => x.Name);
        }
    }
}
