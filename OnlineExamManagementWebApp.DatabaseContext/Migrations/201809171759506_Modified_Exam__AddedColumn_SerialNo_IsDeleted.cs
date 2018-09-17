namespace OnlineExamManagementWebApp.DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modified_Exam__AddedColumn_SerialNo_IsDeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exams", "SerialNo", c => c.Int());
            AddColumn("dbo.Exams", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exams", "IsDeleted");
            DropColumn("dbo.Exams", "SerialNo");
        }
    }
}
