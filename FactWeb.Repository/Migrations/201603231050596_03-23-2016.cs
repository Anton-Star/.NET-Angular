namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _03232016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSection", "ApplicationSectionOrder", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSection", "ApplicationSectionOrder");
        }
    }
}
