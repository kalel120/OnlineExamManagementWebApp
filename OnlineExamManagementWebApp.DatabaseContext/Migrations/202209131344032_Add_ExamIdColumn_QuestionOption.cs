namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ExamIdColumn_QuestionOption : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionOptions", "ExamId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionOptions", "ExamId");
        }
    }
}
