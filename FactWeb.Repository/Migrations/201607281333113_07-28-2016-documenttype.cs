namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _07282016documenttype : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentType",
                c => new
                    {
                        DocumentTypeId = c.Int(nullable: false, identity: true),
                        DocumentTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocumentTypeId);
            
            AddColumn("dbo.Document", "DocumentTypeId", c => c.Int());
            CreateIndex("dbo.Document", "DocumentTypeId");
            AddForeignKey("dbo.Document", "DocumentTypeId", "dbo.DocumentType", "DocumentTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Document", "DocumentTypeId", "dbo.DocumentType");
            DropIndex("dbo.Document", new[] { "DocumentTypeId" });
            DropColumn("dbo.Document", "DocumentTypeId");
            DropTable("dbo.DocumentType");
        }
    }
}
