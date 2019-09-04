namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentLibrary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationDocument",
                c => new
                    {
                        OrganizationDocumentId = c.Guid(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        DocumentId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrganizationDocumentId)
                .ForeignKey("dbo.Document", t => t.DocumentId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .Index(t => t.OrganizationId)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.Document",
                c => new
                    {
                        DocumentId = c.Guid(nullable: false),
                        DocumentPath = c.String(nullable: false, maxLength: 2000),
                        DocumentName = c.String(nullable: false, maxLength: 500),
                        DocumentFactStaffOnly = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocumentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrganizationDocument", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.OrganizationDocument", "DocumentId", "dbo.Document");
            DropIndex("dbo.OrganizationDocument", new[] { "DocumentId" });
            DropIndex("dbo.OrganizationDocument", new[] { "OrganizationId" });
            DropTable("dbo.Document");
            DropTable("dbo.OrganizationDocument");
        }
    }
}
