using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using OnlineExamManagementWebApp.DTOs;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {

    public class CourseManager {
        private readonly UnitOfWork _unitOfWork;

        public CourseManager() {
            _unitOfWork = new UnitOfWork();
        }

        #region OrganizationManager
        
        #endregion

        #region TagManager
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



        #endregion

        public bool IsCourseSaved(Course course) {
            var isDuplicateCode = _unitOfWork.Courses.IsDuplicateCode(course.Code, course.OrganizationId);
            if (isDuplicateCode) {
                return false;
            }
            return _unitOfWork.Courses.AddCourse(course);
        }


        public Course GetCourseWithActiveExamsById(int? id) {
            var course = _unitOfWork.Courses.GetCourseById(id);
            var activeExams = _unitOfWork.Exams.GetActiveExamsByCourseId(course.Id);
            course.Exams = activeExams;
            return course;
        }

        public ICollection<Course> SearchCoursesIfOrgIdSelected(SearchCourseDto dto) {
            var coursesByOrganizationId = _unitOfWork.Courses.GetListOfCourseByOrganizationId(dto.OrganizationId);

            if (dto.Name != null) {
                ICollection<Course> coursesByName = _unitOfWork.Courses.GetCoursesLikeName(dto.Name);
                coursesByOrganizationId =
                    coursesByOrganizationId.Where(o => coursesByName.Any(n => n.Id.Equals(o.Id))).ToList();

            }

            if (dto.Code != null) {
                ICollection<Course> coursesByCode = _unitOfWork.Courses.GetCoursesLikeCode(dto.Code);
                coursesByOrganizationId =
                    coursesByOrganizationId.Where(o => coursesByCode.Any(n => n.Id.Equals(o.Id))).ToList();
            }

            return coursesByOrganizationId;
        }

        public ICollection<Course> SearchCourseIfNoOrgIdSelected(SearchCourseDto dto) {
            List<Course> courses = new List<Course>();

            if (IsAllPropertiesNullOrEmptyOrZero(dto)) {
                courses = _unitOfWork.Courses.GetAllCourses().ToList();
            }

            return courses;
        }

        // Using reflection to check object properties
        private bool IsAllPropertiesNullOrEmptyOrZero(object myObject) {
            bool flag = false;

            foreach (PropertyInfo pi in myObject.GetType().GetProperties()) {
                if (pi.PropertyType == typeof(string)) {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value)) {
                        flag = true;
                    }
                }
                else if (pi.PropertyType == typeof(int)) {
                    int value = (int)pi.GetValue(myObject);
                    if (value == 0) {
                        flag = true;
                    }
                }
            }
            return flag;
        }

    }
}
