namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_AspNetUserRoles_tableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AspNetUserRoles", newName: "AppUserRoles");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.AppUserRoles", newName: "AspNetUserRoles");
        }
    }
}
