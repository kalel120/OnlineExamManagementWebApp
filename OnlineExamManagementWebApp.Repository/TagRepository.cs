using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Repository {
    public class TagRepository {
        private readonly ApplicationDbContext _dbContext;

        public TagRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public IEnumerable GetAllTagNames() {
            return _dbContext.Tags.Select(x => x.Name);
        }

        public void AddNewTags(List<Tag> newTags) {
            _dbContext.Tags.AddRange(newTags);
            _dbContext.SaveChanges();
        }

        public Tag GetTagByName(string searchTerm) {
            return _dbContext.Tags.FirstOrDefault(t => t.Name == searchTerm);
        }
    }
}
