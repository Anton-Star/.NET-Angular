namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06172016_428 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inspection",
                c => new
                    {
                        InspectionId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(),
                        SiteApplicationVersionId = c.Guid(),
                        InspectorId = c.Guid(nullable: false),
                        InspectionCommendablePractices = c.String(),
                        InspectionOverallImpressions = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InspectionId)
                .ForeignKey("dbo.Application", t => t.ApplicationId)
                .ForeignKey("dbo.SiteApplicationVersion", t => t.SiteApplicationVersionId)
                .ForeignKey("dbo.User", t => t.InspectorId)
                .Index(t => t.ApplicationId)
                .Index(t => t.SiteApplicationVersionId)
                .Index(t => t.InspectorId);
            
            AddColumn("dbo.Site", "SiteDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inspection", "InspectorId", "dbo.User");
            DropForeignKey("dbo.Inspection", "SiteApplicationVersionId", "dbo.SiteApplicationVersion");
            DropForeignKey("dbo.Inspection", "ApplicationId", "dbo.Application");
            DropIndex("dbo.Inspection", new[] { "InspectorId" });
            DropIndex("dbo.Inspection", new[] { "SiteApplicationVersionId" });
            DropIndex("dbo.Inspection", new[] { "ApplicationId" });
            DropColumn("dbo.Site", "SiteDescription");
            DropTable("dbo.Inspection");
        }
    }
}
