namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Site_Address_Changes_556 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Site", "SiteStreetAddress1", c => c.String());
            AddColumn("dbo.Site", "SiteStreetAddress2", c => c.String());
            DropColumn("dbo.Site", "SiteStreetAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Site", "SiteStreetAddress", c => c.String());
            DropColumn("dbo.Site", "SiteStreetAddress2");
            DropColumn("dbo.Site", "SiteStreetAddress1");
        }
    }
}
