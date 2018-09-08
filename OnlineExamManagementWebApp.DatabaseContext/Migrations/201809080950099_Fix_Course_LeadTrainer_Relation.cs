namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix_Course_LeadTrainer_Relation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "LeadTrainerId", c => c.Int());
            DropColumn("dbo.Trainers", "IsLead");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trainers", "IsLead", c => c.Boolean(nullable: false));
            DropColumn("dbo.Courses", "LeadTrainerId");
        }
    }
}
