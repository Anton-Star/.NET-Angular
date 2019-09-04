namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01042017MultipleDocuments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationResponse", "DocumentId", "dbo.Document");
            DropIndex("dbo.ApplicationResponse", new[] { "DocumentId" });
            AddColumn("dbo.Document", "ApplicationResponseId", c => c.Int());
            CreateIndex("dbo.Document", "ApplicationResponseId");
            AddForeignKey("dbo.Document", "ApplicationResponseId", "dbo.ApplicationResponse", "ApplicationResponseId");
            DropColumn("dbo.ApplicationResponse", "DocumentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationResponse", "DocumentId", c => c.Guid());
            DropForeignKey("dbo.Document", "ApplicationResponseId", "dbo.ApplicationResponse");
            DropIndex("dbo.Document", new[] { "ApplicationResponseId" });
            DropColumn("dbo.Document", "ApplicationResponseId");
            CreateIndex("dbo.ApplicationResponse", "DocumentId");
            AddForeignKey("dbo.ApplicationResponse", "DocumentId", "dbo.Document", "DocumentId");
        }
    }
}
