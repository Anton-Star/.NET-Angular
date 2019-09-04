namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SiteChanges962016 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Site", new[] { "CountryId" });
            AlterColumn("dbo.Facility", "FacilityCIBMTRData", c => c.Int());
            AlterColumn("dbo.Site", "SiteZip", c => c.String());
            AlterColumn("dbo.Site", "CountryId", c => c.Int());
            CreateIndex("dbo.Site", "CountryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Site", new[] { "CountryId" });
            AlterColumn("dbo.Site", "CountryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Site", "SiteZip", c => c.Int(nullable: false));
            AlterColumn("dbo.Facility", "FacilityCIBMTRData", c => c.Int(nullable: false));
            CreateIndex("dbo.Site", "CountryId");
        }
    }
}
