namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05122016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FacilityAccreditationMapping",
                c => new
                    {
                        FacilityAccreditationMappingId = c.Int(nullable: false, identity: true),
                        FacilityId = c.Int(nullable: false),
                        FacilityAccreditationId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FacilityAccreditationMappingId)
                .ForeignKey("dbo.Facility", t => t.FacilityId)
                .ForeignKey("dbo.FacilityAccreditation", t => t.FacilityAccreditationId)
                .Index(t => t.FacilityId)
                .Index(t => t.FacilityAccreditationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FacilityAccreditationMapping", "FacilityAccreditationId", "dbo.FacilityAccreditation");
            DropForeignKey("dbo.FacilityAccreditationMapping", "FacilityId", "dbo.Facility");
            DropIndex("dbo.FacilityAccreditationMapping", new[] { "FacilityAccreditationId" });
            DropIndex("dbo.FacilityAccreditationMapping", new[] { "FacilityId" });
            DropTable("dbo.FacilityAccreditationMapping");
        }
    }
}
