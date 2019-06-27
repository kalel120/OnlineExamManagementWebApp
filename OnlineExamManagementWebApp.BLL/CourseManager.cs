using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {

    public class CourseManager {
        private readonly UnitOfWork _unitOfWork;

        public CourseManager() {
            _unitOfWork = new UnitOfWork();
        }

        public List<SelectListItem> GetAllOrganizations() {
            var organizations = _unitOfWork.Organizations.GetAllOrganizations();
            organizations.Insert(0, new SelectListItem() { Value = "", Text = "--Select Organization--" });
            return organizations;
        }

        public IEnumerable GetAllTags() {
            return _unitOfWork.Tags.GetAllTagNames();
        }

        public List<Tag> GetReleventTags(List<string> tagList) {
            var newTags = GetNewTags(tagList);
            if (newTags.Count > 0) {
                _unitOfWork.Tags.AddNewTags(newTags);
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

        private List<Tag> GetNewTags(List<string> tags) {
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
            return _unitOfWork.Tags.GetTagByName(searchTerm);
        }

        public bool IsCourseSaved(Course course) {
            var isDuplicateCode = _unitOfWork.Courses.IsDuplicateCode(course.Code,course.OrganizationId);
            if (isDuplicateCode) {
                return false;
            }
            return _unitOfWork.Courses.AddCourse(course);
        }

        public Organization GetOrganizationById(int organizationId) {
            return _unitOfWork.Organizations.GetOrganizationById(organizationId);
        }
       
        public Course GetCourseWithActiveExamsById(int? id) {       
            var course = _unitOfWork.Courses.GetCourseById(id);
            var activeExams = _unitOfWork.Exams.GetActiveExamsByCourseId(course.Id);
            course.Exams = activeExams;
            return course;
        }

        public List<Course> GetCourseByOrganizationId(int organizationId) {
            return _unitOfWork.Courses.GetListOfCourseByOrganizationId(organizationId);
        }
    }
}
