namespace OnlineExamManagementWebApp.DatabaseContext.Migrations {
    using System.Data.Entity.Migrations;

    public partial class SoftDeleteInTrainerEntityModel : DbMigration {
        public override void Up() {
            AddColumn("dbo.Trainers", "IsDeleted", c => c.Boolean(nullable: false));
        }

        public override void Down() {
            DropColumn("dbo.Trainers", "IsDeleted");
        }
    }
}
