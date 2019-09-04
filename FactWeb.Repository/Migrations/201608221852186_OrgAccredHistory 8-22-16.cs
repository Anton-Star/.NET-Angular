namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrgAccredHistory82216 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationAccreditationHistory",
                c => new
                    {
                        OrganizationAccreditationHistoryId = c.Guid(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        AccreditationStatusId = c.Int(),
                        OrganizationAccreditationHistoryAccreditationDate = c.DateTime(),
                        OrganizationAccreditationHistoryExpirationDate = c.DateTime(),
                        OrganizationAccreditationHistoryExtensionDate = c.DateTime(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrganizationAccreditationHistoryId)
                .ForeignKey("dbo.AccreditationStatus", t => t.AccreditationStatusId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .Index(t => t.OrganizationId)
                .Index(t => t.AccreditationStatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrganizationAccreditationHistory", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.OrganizationAccreditationHistory", "AccreditationStatusId", "dbo.AccreditationStatus");
            DropIndex("dbo.OrganizationAccreditationHistory", new[] { "AccreditationStatusId" });
            DropIndex("dbo.OrganizationAccreditationHistory", new[] { "OrganizationId" });
            DropTable("dbo.OrganizationAccreditationHistory");
        }
    }
}
