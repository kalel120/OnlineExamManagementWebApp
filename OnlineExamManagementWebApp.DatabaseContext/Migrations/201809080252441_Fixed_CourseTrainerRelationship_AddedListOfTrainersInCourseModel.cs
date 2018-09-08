namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixed_CourseTrainerRelationship_AddedListOfTrainersInCourseModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "Trainer_Id", "dbo.Trainers");
            DropIndex("dbo.Courses", new[] { "Trainer_Id" });
            CreateTable(
                "dbo.TrainerCourses",
                c => new
                    {
                        Trainer_Id = c.Int(nullable: false),
                        Course_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trainer_Id, t.Course_Id })
                .ForeignKey("dbo.Trainers", t => t.Trainer_Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .Index(t => t.Trainer_Id)
                .Index(t => t.Course_Id);
            
            DropColumn("dbo.Courses", "Trainer_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Trainer_Id", c => c.Int());
            DropForeignKey("dbo.TrainerCourses", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.TrainerCourses", "Trainer_Id", "dbo.Trainers");
            DropIndex("dbo.TrainerCourses", new[] { "Course_Id" });
            DropIndex("dbo.TrainerCourses", new[] { "Trainer_Id" });
            DropTable("dbo.TrainerCourses");
            CreateIndex("dbo.Courses", "Trainer_Id");
            AddForeignKey("dbo.Courses", "Trainer_Id", "dbo.Trainers", "Id");
        }
    }
}
