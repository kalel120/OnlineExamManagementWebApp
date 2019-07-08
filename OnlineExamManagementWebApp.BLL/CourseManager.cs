﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public ICollection<Course> SearchCoursesByParams(SearchCourseDto dto) {
            ICollection<Course> courses;

            if (!dto.OrganizationId.Equals(0)) {
                courses = _unitOfWork.Courses.GetListOfCourseByOrganizationId(dto.OrganizationId);
            }
            else {
                courses = _unitOfWork.Courses.GetAllCourses();
            }

            if (dto.Name != null) {
                ICollection<Course> coursesByName = _unitOfWork.Courses.GetCoursesLikeName(dto.Name);
                courses = courses.Where(o => coursesByName.Any(n => n.Id.Equals(o.Id))).ToList();

            }

            if (dto.Code != null) {
                ICollection<Course> coursesByCode = _unitOfWork.Courses.GetCoursesLikeCode(dto.Code);
                courses = courses.Where(o => coursesByCode.Any(n => n.Id.Equals(o.Id))).ToList();
            }

            return courses;
        }
    }
}
