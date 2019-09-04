namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02242016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSection", "ApplicationSectionComments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSection", "ApplicationSectionComments");
        }
    }
}
