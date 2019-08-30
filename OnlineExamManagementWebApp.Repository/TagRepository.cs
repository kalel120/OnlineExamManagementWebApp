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

        public Tag GetTagByName(string searchTerm) {
            return _dbContext.Tags.FirstOrDefault(t => t.Name == searchTerm);
        }

        public bool AddNewTag(Tag tag) {
            _dbContext.Tags.Add(tag);
            return _dbContext.SaveChanges() > 0;
        }

        public int InsertAndReturnTagId(string tagText) {
            Tag newTag = _dbContext.Tags.Create();
            newTag.Name = tagText;

            _dbContext.Tags.Add(newTag);
            _dbContext.SaveChanges();

            return newTag.Id;
        }
    }
}
