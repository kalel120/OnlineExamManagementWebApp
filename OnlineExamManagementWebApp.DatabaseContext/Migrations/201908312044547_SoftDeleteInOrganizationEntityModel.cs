namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoftDeleteInOrganizationEntityModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "IsDeleted");
        }
    }
}
