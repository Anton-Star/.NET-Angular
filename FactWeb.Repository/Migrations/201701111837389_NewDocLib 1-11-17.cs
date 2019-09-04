namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewDocLib11117 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationDocumentLibrary",
                c => new
                    {
                        OrganizationDocumentLibraryId = c.Guid(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        OrganizationDocumentLibraryVaultId = c.String(),
                        OrganizationDocumentLibraryCycleNumber = c.Int(nullable: false),
                        OrganizationDocumentLibraryIsCurrent = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrganizationDocumentLibraryId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .Index(t => t.OrganizationId);
            
            AddColumn("dbo.Document", "OrganizationDocumentLibraryId", c => c.Guid());
            CreateIndex("dbo.Document", "OrganizationDocumentLibraryId");
            AddForeignKey("dbo.Document", "OrganizationDocumentLibraryId", "dbo.OrganizationDocumentLibrary", "OrganizationDocumentLibraryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrganizationDocumentLibrary", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.Document", "OrganizationDocumentLibraryId", "dbo.OrganizationDocumentLibrary");
            DropIndex("dbo.OrganizationDocumentLibrary", new[] { "OrganizationId" });
            DropIndex("dbo.Document", new[] { "OrganizationDocumentLibraryId" });
            DropColumn("dbo.Document", "OrganizationDocumentLibraryId");
            DropTable("dbo.OrganizationDocumentLibrary");
        }
    }
}
