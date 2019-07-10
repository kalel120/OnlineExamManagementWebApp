namespace OnlineExamManagementWebApp.DatabaseContext.Migrations {
    using System;
    using System.Data.Entity.Migrations;

    public partial class FixCourseTrainerRelationship : DbMigration {
        public override void Up() {
            CreateTable(
                    "dbo.CourseTrainers",
                    c => new {
                        CourseId = c.Int(nullable: false),
                        TrainerId = c.Int(nullable: false),
                        IsLead = c.Boolean(nullable: false)
                    })
                .PrimaryKey(t => new { t.CourseId, t.TrainerId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: false)
                .ForeignKey("dbo.Trainers", t => t.TrainerId, cascadeDelete: false)
                .Index(t => t.CourseId)
                .Index(t => t.TrainerId);
        }

        public override void Down() {
            DropForeignKey("dbo.CourseTrainers", "TrainerId", "dbo.Trainers");
            DropForeignKey("dbo.CourseTrainers", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseTrainers", new[] { "TrainerId" });
            DropIndex("dbo.CourseTrainers", new[] { "CourseId" });
            DropTable("dbo.CourseTrainers");
        }
    }
}
