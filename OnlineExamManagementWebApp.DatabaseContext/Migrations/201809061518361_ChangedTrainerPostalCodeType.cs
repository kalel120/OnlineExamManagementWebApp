namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTrainerPostalCodeType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Trainers", "PostalCode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Trainers", "PostalCode", c => c.Int(nullable: false));
        }
    }
}
