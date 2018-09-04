using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {

    public class CourseManager {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public List<SelectListItem> GetAllOrganizations() {
            var organizations = _unitOfWork._courseRepository.GetAllOrganizations();
            organizations.Insert(0, new SelectListItem() { Value = "", Text = "--Select Organization--" });
            return organizations;
        }

        public IEnumerable GetAllTags() {
            return _unitOfWork._tagRepository.GetAllTagNames();
        }

        public List<Tag> GetReleventTags(List<string> tagList) {
            var newTags = GetNewTags(tagList);
            if (newTags.Count > 0) {
                _unitOfWork._tagRepository.AddNewTags(newTags);
            }

            var courseTags = new List<Tag>();
            foreach (var item in tagList) {
                var tag = GetTagByName(item);
                if (tag != null) {
                    courseTags.Add(tag);
                }
            }
            return courseTags;
        }

        public List<Tag> GetNewTags(List<string> tags) {
            var newTags = new List<Tag>();
            foreach (var item in tags) {
                var tag = GetTagByName(item);
                if (tag == null) {
                    newTags.Add(new Tag { Name = item });
                }
            }
            return newTags;
        }

        private Tag GetTagByName(string searchTerm) {
            return _unitOfWork._tagRepository.GetTagByName(searchTerm);
        }

        public bool IsCourseSaved(Course course) {
            return _unitOfWork._courseRepository.AddCourse(course);
        }
    }
}
