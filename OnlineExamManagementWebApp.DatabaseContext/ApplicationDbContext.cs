using System.Data.Entity;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.DatabaseContext {
    public class ApplicationDbContext : DbContext {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<CourseTrainer> CourseTrainers { get; set; }
    }
}
