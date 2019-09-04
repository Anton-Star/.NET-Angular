namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacilityChange : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Facility", "FacilityCIBMTRData");
            DropColumn("dbo.Facility", "FacilityCIBMTROutcomes");
            DropColumn("dbo.Facility", "FacilityConflictOfInterest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Facility", "FacilityConflictOfInterest", c => c.Boolean(nullable: false));
            AddColumn("dbo.Facility", "FacilityCIBMTROutcomes", c => c.Int(nullable: false));
            AddColumn("dbo.Facility", "FacilityCIBMTRData", c => c.Int());
        }
    }
}
