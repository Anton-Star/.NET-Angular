namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocLibChanges4417 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Document", "ApplicationResponseId", "dbo.ApplicationResponse");
            DropIndex("dbo.Document", new[] { "ApplicationResponseId" });
            AddColumn("dbo.ApplicationResponse", "DocumentId", c => c.Guid());
            CreateIndex("dbo.ApplicationResponse", "DocumentId");
            AddForeignKey("dbo.ApplicationResponse", "DocumentId", "dbo.Document", "DocumentId");
            DropColumn("dbo.Document", "ApplicationResponseId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Document", "ApplicationResponseId", c => c.Int());
            DropForeignKey("dbo.ApplicationResponse", "DocumentId", "dbo.Document");
            DropIndex("dbo.ApplicationResponse", new[] { "DocumentId" });
            DropColumn("dbo.ApplicationResponse", "DocumentId");
            CreateIndex("dbo.Document", "ApplicationResponseId");
            AddForeignKey("dbo.Document", "ApplicationResponseId", "dbo.ApplicationResponse", "ApplicationResponseId");
        }
    }
}
