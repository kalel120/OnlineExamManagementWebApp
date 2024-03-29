﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamManagementWebApp.Models {
    public class Organization {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [MaxLength(100)]
        public string Contact { get; set; }

        public string About { get; set; }

        public byte[] Logo { get; set; }

        public bool IsDeleted { get; set; }

        public List<Course> Courses { get; set; }

        public List<Trainer> Trainers { get; set; }
    }
}