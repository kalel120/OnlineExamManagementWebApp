namespace OnlineExamManagementWebApp.DatabaseContext.Migrations {
    using System;
    using System.Data.Entity.Migrations;

    public partial class Added_NavigationProperty_CourseTrainer : DbMigration {
        public override void Up() {
            CreateTable(
                "dbo.CourseTrainers",
                c => new {
                    CourseId = c.Int(nullable: false),
                    TrainerId = c.Int(nullable: false),
                    IsLead = c.Boolean(nullable: false)

                })
                .PrimaryKey(t => new { t.TrainerId, t.CourseId })
                .ForeignKey("dbo.Trainers", t => t.TrainerId, cascadeDelete: false)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: false)
                .Index(t => t.TrainerId)
                .Index(t => t.CourseId);

        }

        public override void Down() {
            DropForeignKey("dbo.CourseTrainers", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CourseTrainers", "TrainerId", "dbo.Trainers");
            DropIndex("dbo.CourseTrainers", new[] { "CourseId" });
            DropIndex("dbo.CourseTrainers", new[] { "TrainerId" });
            DropTable("dbo.CourseTrainers");
        }
    }
}
