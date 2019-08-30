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

        public ICollection<Tag> GetSelectedTags(List<string> listOfString) {
            return CheckAndInsertAsNewTag(listOfString);
        }

        private ICollection<Tag> CheckAndInsertAsNewTag(List<string> listOfString) {
            var tags = new List<Tag>();

            foreach (var searchTerm in listOfString) {
                var tag = GetTagByName(searchTerm);
                if (tag == null) {
                    var newTag = new Tag { Name = searchTerm };
                    if (_unitOfWork.Tags.AddNewTag(newTag)) {
                        tags.Add(newTag);
                    }
                    tags.Add(newTag);
                }
                else {
                    tags.Add(tag);
                }
            }

            return tags;
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
}