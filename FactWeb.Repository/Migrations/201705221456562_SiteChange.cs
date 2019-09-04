namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SiteChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Site", "CountryId", "dbo.Country");
            DropForeignKey("dbo.Site", "StateId", "dbo.State");
            DropIndex("dbo.Site", new[] { "StateId" });
            DropIndex("dbo.Site", new[] { "CountryId" });
            DropColumn("dbo.Site", "SitePhone");
            DropColumn("dbo.Site", "SiteStreetAddress1");
            DropColumn("dbo.Site", "SiteStreetAddress2");
            DropColumn("dbo.Site", "SiteCity");
            DropColumn("dbo.Site", "SiteProvince");
            DropColumn("dbo.Site", "StateId");
            DropColumn("dbo.Site", "SiteZip");
            DropColumn("dbo.Site", "CountryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Site", "CountryId", c => c.Int());
            AddColumn("dbo.Site", "SiteZip", c => c.String());
            AddColumn("dbo.Site", "StateId", c => c.Int());
            AddColumn("dbo.Site", "SiteProvince", c => c.String());
            AddColumn("dbo.Site", "SiteCity", c => c.String());
            AddColumn("dbo.Site", "SiteStreetAddress2", c => c.String());
            AddColumn("dbo.Site", "SiteStreetAddress1", c => c.String());
            AddColumn("dbo.Site", "SitePhone", c => c.String());
            CreateIndex("dbo.Site", "CountryId");
            CreateIndex("dbo.Site", "StateId");
            AddForeignKey("dbo.Site", "StateId", "dbo.State", "StateId");
            AddForeignKey("dbo.Site", "CountryId", "dbo.Country", "CountryId");
        }
    }
}
