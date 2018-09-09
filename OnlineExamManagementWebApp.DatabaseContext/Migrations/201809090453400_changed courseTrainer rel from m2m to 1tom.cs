namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedcourseTrainerrelfromm2mto1tom : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TrainerCourses", "Trainer_Id", "dbo.Trainers");
            DropForeignKey("dbo.TrainerCourses", "Course_Id", "dbo.Courses");
            DropIndex("dbo.TrainerCourses", new[] { "Trainer_Id" });
            DropIndex("dbo.TrainerCourses", new[] { "Course_Id" });
            CreateTable(
                "dbo.CourseTrainers",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        TrainerId = c.Int(nullable: false),
                        IsLead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseId, t.TrainerId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: false)
                .ForeignKey("dbo.Trainers", t => t.TrainerId, cascadeDelete: false)
                .Index(t => t.CourseId)
                .Index(t => t.TrainerId);
            
            DropTable("dbo.TrainerCourses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TrainerCourses",
                c => new
                    {
                        Trainer_Id = c.Int(nullable: false),
                        Course_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trainer_Id, t.Course_Id });
            
            DropForeignKey("dbo.CourseTrainers", "TrainerId", "dbo.Trainers");
            DropForeignKey("dbo.CourseTrainers", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseTrainers", new[] { "TrainerId" });
            DropIndex("dbo.CourseTrainers", new[] { "CourseId" });
            DropTable("dbo.CourseTrainers");
            CreateIndex("dbo.TrainerCourses", "Course_Id");
            CreateIndex("dbo.TrainerCourses", "Trainer_Id");
            AddForeignKey("dbo.TrainerCourses", "Course_Id", "dbo.Courses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TrainerCourses", "Trainer_Id", "dbo.Trainers", "Id", cascadeDelete: true);
        }
    }
}
