namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganizationBAADocument_11_9_2016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationBAADocument",
                c => new
                    {
                        OrganizationBAADocumentId = c.Guid(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        DocumentId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrganizationBAADocumentId)
                .ForeignKey("dbo.Document", t => t.DocumentId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .Index(t => t.OrganizationId)
                .Index(t => t.DocumentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrganizationBAADocument", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.OrganizationBAADocument", "DocumentId", "dbo.Document");
            DropIndex("dbo.OrganizationBAADocument", new[] { "DocumentId" });
            DropIndex("dbo.OrganizationBAADocument", new[] { "OrganizationId" });
            DropTable("dbo.OrganizationBAADocument");
        }
    }
}
