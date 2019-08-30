using System.Collections.Generic;
using System.Linq;
using OnlineExamManagementWebApp.DTOs;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {

    public class CourseManager {
        private readonly UnitOfWork _unitOfWork;

        public CourseManager() {
            _unitOfWork = new UnitOfWork();
        }

        #region TagManager
        public ICollection<TagDto> GetAllTags() {
            return _unitOfWork.Tags.GetAllTags();
        }

        public List<Tag> GetSelectedTags(List<string> tagText) {
            return _unitOfWork.Tags.GetTagsByIds(GetTagIdsByText(tagText));
        }

        private List<int> GetTagIdsByText(List<string> tagText) {
            var existingTagIds = new List<int>();
            var newTags = new List<string>();

            foreach (var item in tagText) {
                if (int.TryParse(item, out var tagId)) {
                    existingTagIds.Add(tagId);
                }
                else {
                    newTags.Add(item);
                }
            }

            foreach (var item in newTags) {
                existingTagIds.Add(_unitOfWork.Tags.InsertAndReturnTagId(item));
            }

            return existingTagIds;
        }
        #endregion

        #region CourseManager
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

        public ICollection<Course> SearchCoursesByParams(SearchCourseDto dto) {
            ICollection<Course> courses;

            if (!dto.OrganizationId.Equals(0)) {
                courses = _unitOfWork.Courses.GetListOfCourseByOrganizationId(dto.OrganizationId);
            }
            else {
                courses = _unitOfWork.Courses.GetAllActiveCourses();
            }

            if (dto.Name != null) {
                ICollection<Course> coursesByName = _unitOfWork.Courses.GetCoursesLikeName(dto.Name);
                courses = courses.Where(o => coursesByName.Any(n => n.Id.Equals(o.Id))).ToList();

            }

            if (dto.Code != null) {
                ICollection<Course> coursesByCode = _unitOfWork.Courses.GetCoursesLikeCode(dto.Code);
                courses = courses.Where(o => coursesByCode.Any(n => n.Id.Equals(o.Id))).ToList();
            }

            if (dto.DurationFrom != null || dto.DurationTo != null) {
                ICollection<Course> courseByDuration = _unitOfWork.Courses.GetCourseByRangedDuration(dto.DurationFrom, dto.DurationTo);
                courses = courses.Where(o => courseByDuration.Any(n => n.Id.Equals(o.Id))).ToList();
            }

            return courses;
        }

        public bool IsCourseUpdated(Course course) {
            return _unitOfWork.Courses.IsCourseUpdated(course);
        }

        public bool IsCourseDeleted(int courseId) {
            return _unitOfWork.Courses.IsCourseDeleted(courseId);
        }

        public ICollection<CourseWithOrgNameDto> GetAllActiveCoursesWithOrganization() {
            return _unitOfWork.Courses.GetAllActiveCoursesWithOrganization();
        }
    }
    #endregion
}