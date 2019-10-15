using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Models.Identity;

namespace OnlineExamManagementWebApp.DatabaseContext {
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim> {

        public ApplicationDbContext() : base("ApplicationDbContext") {

        }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<CourseTrainer> CourseTrainers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().ToTable("AppUsers");
            modelBuilder.Entity<AppRole>().ToTable("AppRoles");
            modelBuilder.Entity<AppUserLogin>().ToTable("AppUserLogins");
            modelBuilder.Entity<AppUserClaim>().ToTable("AppUserClaims");
        }
    }
}