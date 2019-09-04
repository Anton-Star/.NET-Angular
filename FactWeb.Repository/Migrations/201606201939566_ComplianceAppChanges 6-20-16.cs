namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ComplianceAppChanges62016 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SiteApplicationVersion", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.SiteApplicationVersion", "ApplicationStatusId", "dbo.ApplicationStatus");
            DropForeignKey("dbo.SiteApplicationVersion", "ApplicationVersionId", "dbo.ApplicationVersion");
            DropForeignKey("dbo.SiteApplicationVersionQuestionNotApplicable", "ApplicationSectionQuestionId", "dbo.ApplicationSectionQuestion");
            DropForeignKey("dbo.SiteApplicationVersionQuestionNotApplicable", "SiteApplicationVersionId", "dbo.SiteApplicationVersion");
            DropForeignKey("dbo.SiteApplicationVersion", "SiteId", "dbo.Site");
            DropForeignKey("dbo.Inspection", "SiteApplicationVersionId", "dbo.SiteApplicationVersion");
            DropIndex("dbo.SiteApplicationVersion", new[] { "ApplicationId" });
            DropIndex("dbo.SiteApplicationVersion", new[] { "SiteId" });
            DropIndex("dbo.SiteApplicationVersion", new[] { "ApplicationVersionId" });
            DropIndex("dbo.SiteApplicationVersion", new[] { "ApplicationStatusId" });
            DropIndex("dbo.SiteApplicationVersionQuestionNotApplicable", new[] { "SiteApplicationVersionId" });
            DropIndex("dbo.SiteApplicationVersionQuestionNotApplicable", new[] { "ApplicationSectionQuestionId" });
            DropIndex("dbo.Inspection", new[] { "SiteApplicationVersionId" });
            CreateTable(
                "dbo.ApplicationQuestionNotApplicable",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        ApplicationSectionQuestionId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Application", t => t.ApplicationId)
                .ForeignKey("dbo.ApplicationSectionQuestion", t => t.ApplicationSectionQuestionId)
                .Index(t => t.ApplicationId)
                .Index(t => t.ApplicationSectionQuestionId);
            
            AddColumn("dbo.Application", "SiteId", c => c.Int());
            CreateIndex("dbo.Application", "SiteId");
            AddForeignKey("dbo.Application", "SiteId", "dbo.Site", "SiteId");
            DropColumn("dbo.Inspection", "SiteApplicationVersionId");
            DropTable("dbo.SiteApplicationVersion");
            DropTable("dbo.SiteApplicationVersionQuestionNotApplicable");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SiteApplicationVersionQuestionNotApplicable",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SiteApplicationVersionId = c.Guid(nullable: false),
                        ApplicationSectionQuestionId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SiteApplicationVersion",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ApplicationVersionId = c.Guid(nullable: false),
                        ApplicationStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Inspection", "SiteApplicationVersionId", c => c.Guid());
            DropForeignKey("dbo.Application", "SiteId", "dbo.Site");
            DropForeignKey("dbo.ApplicationQuestionNotApplicable", "ApplicationSectionQuestionId", "dbo.ApplicationSectionQuestion");
            DropForeignKey("dbo.ApplicationQuestionNotApplicable", "ApplicationId", "dbo.Application");
            DropIndex("dbo.ApplicationQuestionNotApplicable", new[] { "ApplicationSectionQuestionId" });
            DropIndex("dbo.ApplicationQuestionNotApplicable", new[] { "ApplicationId" });
            DropIndex("dbo.Application", new[] { "SiteId" });
            DropColumn("dbo.Application", "SiteId");
            DropTable("dbo.ApplicationQuestionNotApplicable");
            CreateIndex("dbo.Inspection", "SiteApplicationVersionId");
            CreateIndex("dbo.SiteApplicationVersionQuestionNotApplicable", "ApplicationSectionQuestionId");
            CreateIndex("dbo.SiteApplicationVersionQuestionNotApplicable", "SiteApplicationVersionId");
            CreateIndex("dbo.SiteApplicationVersion", "ApplicationStatusId");
            CreateIndex("dbo.SiteApplicationVersion", "ApplicationVersionId");
            CreateIndex("dbo.SiteApplicationVersion", "SiteId");
            CreateIndex("dbo.SiteApplicationVersion", "ApplicationId");
            AddForeignKey("dbo.Inspection", "SiteApplicationVersionId", "dbo.SiteApplicationVersion", "Id");
            AddForeignKey("dbo.SiteApplicationVersion", "SiteId", "dbo.Site", "SiteId");
            AddForeignKey("dbo.SiteApplicationVersionQuestionNotApplicable", "SiteApplicationVersionId", "dbo.SiteApplicationVersion", "Id");
            AddForeignKey("dbo.SiteApplicationVersionQuestionNotApplicable", "ApplicationSectionQuestionId", "dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionId");
            AddForeignKey("dbo.SiteApplicationVersion", "ApplicationVersionId", "dbo.ApplicationVersion", "ApplicationVersionId");
            AddForeignKey("dbo.SiteApplicationVersion", "ApplicationStatusId", "dbo.ApplicationStatus", "ApplicationStatusId");
            AddForeignKey("dbo.SiteApplicationVersion", "ApplicationId", "dbo.Application", "ApplicationId");
        }
    }
}
