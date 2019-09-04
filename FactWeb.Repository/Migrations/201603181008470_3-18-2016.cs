namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3182016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScopeType",
                c => new
                    {
                        ScopeTypeId = c.Int(nullable: false, identity: true),
                        ScopeTypeName = c.String(),
                        ScopeTypeImportName = c.String(),
                        ScopeTypeIsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ScopeTypeId);
            
            AddColumn("dbo.InspectionScheduleFacility", "InspectionScheduleFacilityInspectionDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailInspectionDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailInspectionDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.InspectionScheduleFacility", "InspectionScheduleFacilityInspectionDate");
            DropTable("dbo.ScopeType");
        }
    }
}
