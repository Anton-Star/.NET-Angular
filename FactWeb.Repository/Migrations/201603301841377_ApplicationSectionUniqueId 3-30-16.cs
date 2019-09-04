namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationSectionUniqueId33016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSection", "ApplicationSectionUniqueIdentifier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSection", "ApplicationSectionUniqueIdentifier");
        }
    }
}
