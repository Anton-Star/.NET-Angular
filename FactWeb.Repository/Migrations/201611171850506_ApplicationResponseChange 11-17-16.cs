namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationResponseChange111716 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponse", "VisibleApplicationResponseStatusId", c => c.Int());
            CreateIndex("dbo.ApplicationResponse", "VisibleApplicationResponseStatusId");
            AddForeignKey("dbo.ApplicationResponse", "VisibleApplicationResponseStatusId", "dbo.ApplicationResponseStatus", "ApplicationResponseStatus");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationResponse", "VisibleApplicationResponseStatusId", "dbo.ApplicationResponseStatus");
            DropIndex("dbo.ApplicationResponse", new[] { "VisibleApplicationResponseStatusId" });
            DropColumn("dbo.ApplicationResponse", "VisibleApplicationResponseStatusId");
        }
    }
}
