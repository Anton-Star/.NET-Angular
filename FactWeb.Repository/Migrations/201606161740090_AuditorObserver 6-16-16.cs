namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AuditorObserver61616 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserIsAuditor", c => c.Boolean());
            AddColumn("dbo.User", "UserIsObserver", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UserIsObserver");
            DropColumn("dbo.User", "UserIsAuditor");
        }
    }
}
