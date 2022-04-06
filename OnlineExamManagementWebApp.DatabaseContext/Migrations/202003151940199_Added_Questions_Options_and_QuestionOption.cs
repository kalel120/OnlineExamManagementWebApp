namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Questions_Options_and_QuestionOption : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionOptions",
                c => new
                    {
                        QuestionId = c.Guid(nullable: false),
                        OptionId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsCorrectAnswer = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuestionId, t.OptionId })
                .ForeignKey("dbo.Options", t => t.OptionId, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.OptionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        OptionType = c.String(),
                        OptionOneId = c.Guid(nullable: false),
                        OptionTwoId = c.Guid(nullable: false),
                        OptionThreeId = c.Guid(nullable: false),
                        OptionFourId = c.Guid(nullable: false),
                        Marks = c.Int(nullable: false),
                        Serial = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ExamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exams", t => t.ExamId, cascadeDelete: true)
                .Index(t => t.ExamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionOptions", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Questions", "ExamId", "dbo.Exams");
            DropForeignKey("dbo.QuestionOptions", "OptionId", "dbo.Options");
            DropIndex("dbo.Questions", new[] { "ExamId" });
            DropIndex("dbo.QuestionOptions", new[] { "OptionId" });
            DropIndex("dbo.QuestionOptions", new[] { "QuestionId" });
            DropTable("dbo.Questions");
            DropTable("dbo.QuestionOptions");
            DropTable("dbo.Options");
        }
    }
}
