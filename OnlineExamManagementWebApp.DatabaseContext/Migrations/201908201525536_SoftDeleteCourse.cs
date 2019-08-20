namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoftDeleteCourse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "IsDeleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Courses", "LeadTrainerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "LeadTrainerId", c => c.Int());
            DropColumn("dbo.Courses", "IsDeleted");
        }
    }
}
