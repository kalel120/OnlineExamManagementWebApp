using System.Collections.Generic;
using System.Linq;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.DTOs;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Repository {
    public class TagRepository {
        private readonly ApplicationDbContext _dbContext;

        public TagRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public ICollection<TagDto> GetAllTags() {
            return _dbContext.Tags.Select(t => new TagDto {
                Id = t.Id,
                Name = t.Name
            }).ToList();
        }

        public int InsertAndReturnTagId(string tagText) {
            Tag newTag = _dbContext.Tags.Create();
            newTag.Name = tagText;

            _dbContext.Tags.Add(newTag);
            _dbContext.SaveChanges();

            return newTag.Id;
        }

        public List<Tag> GetTagsByIds(List<int> ids) {
            var tags = _dbContext.Tags.Where(t => ids.Contains(t.Id)).ToList();
            return tags;
        }
    }
}
