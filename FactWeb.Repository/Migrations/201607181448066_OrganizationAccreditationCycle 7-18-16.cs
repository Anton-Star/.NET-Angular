namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganizationAccreditationCycle71816 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationAccreditationCycle",
                c => new
                    {
                        OrganizationAccreditationCycleId = c.Guid(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        OrganizationAccreditationCycleNumber = c.Int(nullable: false),
                        OrganizationAccreditationCycleEffectiveDate = c.DateTime(nullable: false),
                        OrganizationAccreditationCycleIsCurrent = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrganizationAccreditationCycleId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .Index(t => t.OrganizationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrganizationAccreditationCycle", "OrganizationId", "dbo.Organization");
            DropIndex("dbo.OrganizationAccreditationCycle", new[] { "OrganizationId" });
            DropTable("dbo.OrganizationAccreditationCycle");
        }
    }
}
