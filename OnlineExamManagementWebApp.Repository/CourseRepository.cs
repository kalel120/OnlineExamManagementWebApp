﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OnlineExamManagementWebApp.DatabaseContext;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.Repository {
    public class CourseRepository {
        private readonly ApplicationDbContext _dbContext;

        public CourseRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public List<SelectListItem> GetAllOrganizations() {
            return _dbContext.Organizations.Select(c => new SelectListItem() {Value = c.Id.ToString(), Text = c.Code})
                .ToList();
        }

        public bool AddCourse(Course course) {
            _dbContext.Courses.Add(course);
            return _dbContext.SaveChanges() > 0;
        }

        public bool IsDuplicateCode(string courseCode, int organizationId) {
            Course course = _dbContext.Courses.Where(c => c.Code == courseCode & c.OrganizationId== organizationId).FirstOrDefault();
            if (course==null) {
                return false;
            }

            return true;
        }

       
    }
}