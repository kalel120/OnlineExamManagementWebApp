namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_AttributeSettings_Organization : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Organizations", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Organizations", "Code", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Organizations", "Address", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Organizations", "Contact", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Organizations", "Contact", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Organizations", "Address", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Organizations", "Code", c => c.String());
            AlterColumn("dbo.Organizations", "Name", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
