namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _032420161 : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.ApplicationSection", "ApplicationSectionOrder", c => c.String());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.ApplicationSection", "ApplicationSectionOrder", c => c.Int(nullable: false));
        }
    }
}
