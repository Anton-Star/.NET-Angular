namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _51316 : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationSectionQuestion", t => t.ApplicationSectionQuestionId)
                .ForeignKey("dbo.SiteApplicationVersion", t => t.SiteApplicationVersionId)
                .Index(t => t.SiteApplicationVersionId)
                .Index(t => t.ApplicationSectionQuestionId);
            
            AddColumn("dbo.ComplianceApplication", "ComplianceApplicationRejectionComments", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiteApplicationVersionQuestionNotApplicable", "SiteApplicationVersionId", "dbo.SiteApplicationVersion");
            DropForeignKey("dbo.SiteApplicationVersionQuestionNotApplicable", "ApplicationSectionQuestionId", "dbo.ApplicationSectionQuestion");
            DropIndex("dbo.SiteApplicationVersionQuestionNotApplicable", new[] { "ApplicationSectionQuestionId" });
            DropIndex("dbo.SiteApplicationVersionQuestionNotApplicable", new[] { "SiteApplicationVersionId" });
            DropColumn("dbo.ComplianceApplication", "ComplianceApplicationRejectionComments");
            DropTable("dbo.SiteApplicationVersionQuestionNotApplicable");
        }
    }
}
