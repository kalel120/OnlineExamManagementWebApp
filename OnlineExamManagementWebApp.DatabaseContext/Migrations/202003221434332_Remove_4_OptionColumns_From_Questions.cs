namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_4_OptionColumns_From_Questions : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Questions", "OptionOneId");
            DropColumn("dbo.Questions", "OptionTwoId");
            DropColumn("dbo.Questions", "OptionThreeId");
            DropColumn("dbo.Questions", "OptionFourId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "OptionFourId", c => c.Guid(nullable: false));
            AddColumn("dbo.Questions", "OptionThreeId", c => c.Guid(nullable: false));
            AddColumn("dbo.Questions", "OptionTwoId", c => c.Guid(nullable: false));
            AddColumn("dbo.Questions", "OptionOneId", c => c.Guid(nullable: false));
        }
    }
}
