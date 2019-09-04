namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04212016 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InspectionScheduleDetail", "FacilityId", "dbo.Facility");
            DropForeignKey("dbo.InspectionScheduleFacility", "FacilityID", "dbo.Facility");
            DropForeignKey("dbo.InspectionScheduleFacility", "InspectionScheduleId", "dbo.InspectionSchedule");
            DropIndex("dbo.InspectionScheduleDetail", new[] { "FacilityId" });
            DropIndex("dbo.InspectionScheduleFacility", new[] { "FacilityID" });
            DropIndex("dbo.InspectionScheduleFacility", new[] { "InspectionScheduleId" });
            CreateTable(
                "dbo.InspectionScheduleSite",
                c => new
                    {
                        InspectionScheduleFacilityId = c.Int(nullable: false, identity: true),
                        SiteID = c.Int(nullable: false),
                        InspectionScheduleId = c.Int(nullable: false),
                        InspectionScheduleFacilityInspectionDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InspectionScheduleFacilityId)
                .ForeignKey("dbo.InspectionSchedule", t => t.InspectionScheduleId)
                .ForeignKey("dbo.Site", t => t.SiteID)
                .Index(t => t.SiteID)
                .Index(t => t.InspectionScheduleId);
            
            AddColumn("dbo.InspectionScheduleDetail", "SiteId", c => c.Int());
            CreateIndex("dbo.InspectionScheduleDetail", "SiteId");
            AddForeignKey("dbo.InspectionScheduleDetail", "SiteId", "dbo.Site", "SiteId");
            DropColumn("dbo.InspectionScheduleDetail", "FacilityId");
            DropTable("dbo.InspectionScheduleFacility");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InspectionScheduleFacility",
                c => new
                    {
                        InspectionScheduleFacilityId = c.Int(nullable: false, identity: true),
                        FacilityID = c.Int(nullable: false),
                        InspectionScheduleId = c.Int(nullable: false),
                        InspectionScheduleFacilityInspectionDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InspectionScheduleFacilityId);
            
            AddColumn("dbo.InspectionScheduleDetail", "FacilityId", c => c.Int(nullable: false));
            DropForeignKey("dbo.InspectionScheduleSite", "SiteID", "dbo.Site");
            DropForeignKey("dbo.InspectionScheduleSite", "InspectionScheduleId", "dbo.InspectionSchedule");
            DropForeignKey("dbo.InspectionScheduleDetail", "SiteId", "dbo.Site");
            DropIndex("dbo.InspectionScheduleSite", new[] { "InspectionScheduleId" });
            DropIndex("dbo.InspectionScheduleSite", new[] { "SiteID" });
            DropIndex("dbo.InspectionScheduleDetail", new[] { "SiteId" });
            DropColumn("dbo.InspectionScheduleDetail", "SiteId");
            DropTable("dbo.InspectionScheduleSite");
            CreateIndex("dbo.InspectionScheduleFacility", "InspectionScheduleId");
            CreateIndex("dbo.InspectionScheduleFacility", "FacilityID");
            CreateIndex("dbo.InspectionScheduleDetail", "FacilityId");
            AddForeignKey("dbo.InspectionScheduleFacility", "InspectionScheduleId", "dbo.InspectionSchedule", "InspectionScheduleId");
            AddForeignKey("dbo.InspectionScheduleFacility", "FacilityID", "dbo.Facility", "FacilityId");
            AddForeignKey("dbo.InspectionScheduleDetail", "FacilityId", "dbo.Facility", "FacilityId");
        }
    }
}
