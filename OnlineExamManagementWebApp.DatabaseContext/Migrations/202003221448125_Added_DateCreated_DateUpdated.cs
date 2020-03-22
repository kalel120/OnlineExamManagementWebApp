namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_DateCreated_DateUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Options", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Options", "DateUpdated", c => c.DateTime());
            AddColumn("dbo.QuestionOptions", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.QuestionOptions", "DateUpdated", c => c.DateTime());
            AddColumn("dbo.Questions", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Questions", "DateUpdated", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "DateUpdated");
            DropColumn("dbo.Questions", "DateCreated");
            DropColumn("dbo.QuestionOptions", "DateUpdated");
            DropColumn("dbo.QuestionOptions", "DateCreated");
            DropColumn("dbo.Options", "DateUpdated");
            DropColumn("dbo.Options", "DateCreated");
        }
    }
}
