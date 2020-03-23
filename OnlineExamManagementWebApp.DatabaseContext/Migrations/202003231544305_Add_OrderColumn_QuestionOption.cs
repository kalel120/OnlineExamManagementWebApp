namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_OrderColumn_QuestionOption : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionOptions", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionOptions", "Order");
        }
    }
}
