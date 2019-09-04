namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2182016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionScheduleDetail", "FacilityId", c => c.Int(nullable: false));
            CreateIndex("dbo.InspectionScheduleDetail", "FacilityId");
            AddForeignKey("dbo.InspectionScheduleDetail", "FacilityId", "dbo.Facility", "FacilityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InspectionScheduleDetail", "FacilityId", "dbo.Facility");
            DropIndex("dbo.InspectionScheduleDetail", new[] { "FacilityId" });
            DropColumn("dbo.InspectionScheduleDetail", "FacilityId");
        }
    }
}
