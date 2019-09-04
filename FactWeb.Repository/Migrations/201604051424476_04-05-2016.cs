namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _04052016 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ApplicationSection", new[] { "ApplicationVersionId" });
            DropIndex("dbo.ApplicationVersion", new[] { "ApplicationTypeId" });
            CreateTable(
                "dbo.FacilityAccreditation",
                c => new
                    {
                        FacilityAccreditationId = c.Int(nullable: false, identity: true),
                        FacilityAccreditationName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FacilityAccreditationId);
            
            CreateTable(
                "dbo.FacilityType",
                c => new
                    {
                        FacilityTypeId = c.Int(nullable: false, identity: true),
                        FacilityTypeName = c.String(),
                        FacilityTypeShortName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FacilityTypeId);
            
            AddColumn("dbo.Facility", "FacilityDirectorId", c => c.Guid());
            AddColumn("dbo.Facility", "FacilityCIBMTRData", c => c.Int(nullable: false));
            AddColumn("dbo.Facility", "FacilityCIBMTROutcomes", c => c.Int(nullable: false));
            AddColumn("dbo.Facility", "FacilityOtherSiteAccreditationDetails", c => c.String());
            AddColumn("dbo.Facility", "FacilityMaxtrixMax", c => c.String());
            AddColumn("dbo.Facility", "FacilityConflictOfInterest", c => c.Boolean(nullable: false));
            AddColumn("dbo.Facility", "FacilityQMRestrictions", c => c.Boolean(nullable: false));
            AddColumn("dbo.Facility", "FacilityNetCordMembership", c => c.Boolean(nullable: false));
            AddColumn("dbo.Facility", "FacilityHRSA", c => c.Boolean(nullable: false));
            AddColumn("dbo.Facility", "FacilityTypeId", c => c.Int());
            AddColumn("dbo.Facility", "FacilityAccreditationId", c => c.Int());
            AddColumn("dbo.ServiceType", "FacilityTypeId", c => c.Int());
            CreateIndex("dbo.Facility", "FacilityDirectorId");
            CreateIndex("dbo.Facility", "FacilityTypeId");
            CreateIndex("dbo.Facility", "FacilityAccreditationId");
            CreateIndex("dbo.ServiceType", "FacilityTypeId");
            AddForeignKey("dbo.Facility", "FacilityAccreditationId", "dbo.FacilityAccreditation", "FacilityAccreditationId");
            AddForeignKey("dbo.Facility", "FacilityDirectorId", "dbo.User", "UserId");
            AddForeignKey("dbo.Facility", "FacilityTypeId", "dbo.FacilityType", "FacilityTypeId");
            AddForeignKey("dbo.ServiceType", "FacilityTypeId", "dbo.FacilityType", "FacilityTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceType", "FacilityTypeId", "dbo.FacilityType");
            DropForeignKey("dbo.Facility", "FacilityTypeId", "dbo.FacilityType");
            DropForeignKey("dbo.Facility", "FacilityDirectorId", "dbo.User");
            DropForeignKey("dbo.Facility", "FacilityAccreditationId", "dbo.FacilityAccreditation");
            DropIndex("dbo.ServiceType", new[] { "FacilityTypeId" });
            DropIndex("dbo.Facility", new[] { "FacilityAccreditationId" });
            DropIndex("dbo.Facility", new[] { "FacilityTypeId" });
            DropIndex("dbo.Facility", new[] { "FacilityDirectorId" });
            DropColumn("dbo.ServiceType", "FacilityTypeId");
            DropColumn("dbo.Facility", "FacilityAccreditationId");
            DropColumn("dbo.Facility", "FacilityTypeId");
            DropColumn("dbo.Facility", "FacilityHRSA");
            DropColumn("dbo.Facility", "FacilityNetCordMembership");
            DropColumn("dbo.Facility", "FacilityQMRestrictions");
            DropColumn("dbo.Facility", "FacilityConflictOfInterest");
            DropColumn("dbo.Facility", "FacilityMaxtrixMax");
            DropColumn("dbo.Facility", "FacilityOtherSiteAccreditationDetails");
            DropColumn("dbo.Facility", "FacilityCIBMTROutcomes");
            DropColumn("dbo.Facility", "FacilityCIBMTRData");
            DropColumn("dbo.Facility", "FacilityDirectorId");
            DropTable("dbo.FacilityType");
            DropTable("dbo.FacilityAccreditation");
        }
    }
}
