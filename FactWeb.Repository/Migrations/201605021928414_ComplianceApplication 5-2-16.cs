namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComplianceApplication5216 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteApplicationVersion",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ApplicationVersionId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Application", t => t.ApplicationId)
                .ForeignKey("dbo.ApplicationVersion", t => t.ApplicationVersionId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.ApplicationId)
                .Index(t => t.SiteId)
                .Index(t => t.ApplicationVersionId);
            
            CreateTable(
                "dbo.ComplianceApplication",
                c => new
                    {
                        ComplianceApplicationId = c.Guid(nullable: false),
                        ComplianceApplicationApprovalStatusId = c.Guid(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        ApplicationStatusId = c.Int(nullable: false),
                        CoordinatorId = c.Guid(nullable: false),
                        ComplianceApplicationAccreditationGoal = c.String(),
                        ComplianceApplicationInspectionScope = c.String(),
                        ComplianceApplicationIsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ComplianceApplicationId)
                .ForeignKey("dbo.ApplicationStatus", t => t.ApplicationStatusId)
                .ForeignKey("dbo.ComplianceApplicationApprovalStatus", t => t.ComplianceApplicationApprovalStatusId)
                .ForeignKey("dbo.User", t => t.CoordinatorId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .Index(t => t.ComplianceApplicationApprovalStatusId)
                .Index(t => t.OrganizationId)
                .Index(t => t.ApplicationStatusId)
                .Index(t => t.CoordinatorId);
            
            CreateTable(
                "dbo.ComplianceApplicationApprovalStatus",
                c => new
                    {
                        ComplianceApplicationApprovalStatusId = c.Guid(nullable: false),
                        ComplianceApplicationApprovalStatusName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ComplianceApplicationApprovalStatusId);
            
            CreateTable(
                "dbo.Template",
                c => new
                    {
                        TemplateId = c.Guid(nullable: false),
                        TemplateName = c.String(nullable: false, maxLength: 100),
                        TemplateText = c.String(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TemplateId);
            
            AddColumn("dbo.Application", "ComplianceApplicationId", c => c.Guid());
            CreateIndex("dbo.Application", "ComplianceApplicationId");
            AddForeignKey("dbo.Application", "ComplianceApplicationId", "dbo.ComplianceApplication", "ComplianceApplicationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComplianceApplication", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.ComplianceApplication", "CoordinatorId", "dbo.User");
            DropForeignKey("dbo.ComplianceApplication", "ComplianceApplicationApprovalStatusId", "dbo.ComplianceApplicationApprovalStatus");
            DropForeignKey("dbo.ComplianceApplication", "ApplicationStatusId", "dbo.ApplicationStatus");
            DropForeignKey("dbo.Application", "ComplianceApplicationId", "dbo.ComplianceApplication");
            DropForeignKey("dbo.SiteApplicationVersion", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteApplicationVersion", "ApplicationVersionId", "dbo.ApplicationVersion");
            DropForeignKey("dbo.SiteApplicationVersion", "ApplicationId", "dbo.Application");
            DropIndex("dbo.ComplianceApplication", new[] { "CoordinatorId" });
            DropIndex("dbo.ComplianceApplication", new[] { "ApplicationStatusId" });
            DropIndex("dbo.ComplianceApplication", new[] { "OrganizationId" });
            DropIndex("dbo.ComplianceApplication", new[] { "ComplianceApplicationApprovalStatusId" });
            DropIndex("dbo.SiteApplicationVersion", new[] { "ApplicationVersionId" });
            DropIndex("dbo.SiteApplicationVersion", new[] { "SiteId" });
            DropIndex("dbo.SiteApplicationVersion", new[] { "ApplicationId" });
            DropIndex("dbo.Application", new[] { "ComplianceApplicationId" });
            DropColumn("dbo.Application", "ComplianceApplicationId");
            DropTable("dbo.Template");
            DropTable("dbo.ComplianceApplicationApprovalStatus");
            DropTable("dbo.ComplianceApplication");
            DropTable("dbo.SiteApplicationVersion");
        }
    }
}
