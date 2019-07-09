using System.Collections;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Repository {
    public class TagRepository {
        private readonly ApplicationDbContext _dbContext;

        public TagRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public IEnumerable GetEveryTagName() {
            return _dbContext.Tags.Select(x => x.Name);
        }

        public Tag GetTagByName(string searchTerm) {
            return _dbContext.Tags.FirstOrDefault(t => t.Name == searchTerm);
        }

        public bool AddNewTag(Tag tag) {
            _dbContext.Tags.Add(tag);
            return _dbContext.SaveChanges() > 0;
        }
    }
}
