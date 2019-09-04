namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06212016CommentType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationQuestionNotApplicable", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ApplicationQuestionNotApplicable", "ApplicationSectionQuestionId", "dbo.ApplicationSectionQuestion");
            DropForeignKey("dbo.Application", "SiteId", "dbo.Site");
            DropIndex("dbo.Application", new[] { "SiteId" });
            DropIndex("dbo.ApplicationQuestionNotApplicable", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationQuestionNotApplicable", new[] { "ApplicationSectionQuestionId" });
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Application", t => t.ApplicationId)
                .ForeignKey("dbo.ApplicationStatus", t => t.ApplicationStatusId)
                .ForeignKey("dbo.ApplicationVersion", t => t.ApplicationVersionId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.ApplicationId)
                .Index(t => t.SiteId)
                .Index(t => t.ApplicationVersionId)
                .Index(t => t.ApplicationStatusId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationSectionQuestion", t => t.ApplicationSectionQuestionId)
                .ForeignKey("dbo.SiteApplicationVersion", t => t.SiteApplicationVersionId)
                .Index(t => t.SiteApplicationVersionId)
                .Index(t => t.ApplicationSectionQuestionId);
            
            CreateTable(
                "dbo.CommentType",
                c => new
                    {
                        CommentTypeId = c.Int(nullable: false, identity: true),
                        CommentTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CommentTypeId);
            
            AddColumn("dbo.ApplicationResponseComment", "Comment", c => c.String());
            AddColumn("dbo.ApplicationResponseComment", "CommentTypeId", c => c.Int());
            AddColumn("dbo.Inspection", "SiteApplicationVersionId", c => c.Guid());
            CreateIndex("dbo.ApplicationResponseComment", "CommentTypeId");
            CreateIndex("dbo.Inspection", "SiteApplicationVersionId");
            AddForeignKey("dbo.ApplicationResponseComment", "CommentTypeId", "dbo.CommentType", "CommentTypeId");
            AddForeignKey("dbo.Inspection", "SiteApplicationVersionId", "dbo.SiteApplicationVersion", "Id");
            DropColumn("dbo.Application", "SiteId");
            DropColumn("dbo.ApplicationResponseComment", "ApplicationResponseCommentRFIComment");
            DropColumn("dbo.ApplicationResponseComment", "ApplicationResponseCommentCitationComment");
            DropColumn("dbo.ApplicationResponseComment", "ApplicationResponseCommentCoordinatorComment");
            DropTable("dbo.ApplicationQuestionNotApplicable");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ApplicationResponseComment", "ApplicationResponseCommentCoordinatorComment", c => c.String());
            AddColumn("dbo.ApplicationResponseComment", "ApplicationResponseCommentCitationComment", c => c.String());
            AddColumn("dbo.ApplicationResponseComment", "ApplicationResponseCommentRFIComment", c => c.String());
            AddColumn("dbo.Application", "SiteId", c => c.Int());
            DropForeignKey("dbo.Inspection", "SiteApplicationVersionId", "dbo.SiteApplicationVersion");
            DropForeignKey("dbo.ApplicationResponseComment", "CommentTypeId", "dbo.CommentType");
            DropForeignKey("dbo.SiteApplicationVersion", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteApplicationVersionQuestionNotApplicable", "SiteApplicationVersionId", "dbo.SiteApplicationVersion");
            DropForeignKey("dbo.SiteApplicationVersionQuestionNotApplicable", "ApplicationSectionQuestionId", "dbo.ApplicationSectionQuestion");
            DropForeignKey("dbo.SiteApplicationVersion", "ApplicationVersionId", "dbo.ApplicationVersion");
            DropForeignKey("dbo.SiteApplicationVersion", "ApplicationStatusId", "dbo.ApplicationStatus");
            DropForeignKey("dbo.SiteApplicationVersion", "ApplicationId", "dbo.Application");
            DropIndex("dbo.Inspection", new[] { "SiteApplicationVersionId" });
            DropIndex("dbo.SiteApplicationVersionQuestionNotApplicable", new[] { "ApplicationSectionQuestionId" });
            DropIndex("dbo.SiteApplicationVersionQuestionNotApplicable", new[] { "SiteApplicationVersionId" });
            DropIndex("dbo.SiteApplicationVersion", new[] { "ApplicationStatusId" });
            DropIndex("dbo.SiteApplicationVersion", new[] { "ApplicationVersionId" });
            DropIndex("dbo.SiteApplicationVersion", new[] { "SiteId" });
            DropIndex("dbo.SiteApplicationVersion", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationResponseComment", new[] { "CommentTypeId" });
            DropColumn("dbo.Inspection", "SiteApplicationVersionId");
            DropColumn("dbo.ApplicationResponseComment", "CommentTypeId");
            DropColumn("dbo.ApplicationResponseComment", "Comment");
            DropTable("dbo.CommentType");
            DropTable("dbo.SiteApplicationVersionQuestionNotApplicable");
            DropTable("dbo.SiteApplicationVersion");
            CreateIndex("dbo.ApplicationQuestionNotApplicable", "ApplicationSectionQuestionId");
            CreateIndex("dbo.ApplicationQuestionNotApplicable", "ApplicationId");
            CreateIndex("dbo.Application", "SiteId");
            AddForeignKey("dbo.Application", "SiteId", "dbo.Site", "SiteId");
            AddForeignKey("dbo.ApplicationQuestionNotApplicable", "ApplicationSectionQuestionId", "dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionId");
            AddForeignKey("dbo.ApplicationQuestionNotApplicable", "ApplicationId", "dbo.Application", "ApplicationId");
        }
    }
}
