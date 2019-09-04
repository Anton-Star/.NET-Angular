namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationResponseDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponse", "DocumentId", c => c.Guid());
            CreateIndex("dbo.ApplicationResponse", "DocumentId");
            AddForeignKey("dbo.ApplicationResponse", "DocumentId", "dbo.Document", "DocumentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationResponse", "DocumentId", "dbo.Document");
            DropIndex("dbo.ApplicationResponse", new[] { "DocumentId" });
            DropColumn("dbo.ApplicationResponse", "DocumentId");
        }
    }
}
