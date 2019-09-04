namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComplianceApplication5316 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SiteApplicationVersion", "ApplicationStatusId", c => c.Int(nullable: false));
            CreateIndex("dbo.SiteApplicationVersion", "ApplicationStatusId");
            AddForeignKey("dbo.SiteApplicationVersion", "ApplicationStatusId", "dbo.ApplicationStatus", "ApplicationStatusId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiteApplicationVersion", "ApplicationStatusId", "dbo.ApplicationStatus");
            DropIndex("dbo.SiteApplicationVersion", new[] { "ApplicationStatusId" });
            DropColumn("dbo.SiteApplicationVersion", "ApplicationStatusId");
        }
    }
}
