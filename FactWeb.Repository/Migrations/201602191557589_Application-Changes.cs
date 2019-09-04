namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ApplicationChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationResponse",
                c => new
                    {
                        ApplicationResponseId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        ApplicationSectionQuestionId = c.Guid(nullable: false),
                        ApplicationSectionQuestionAnswerId = c.Guid(),
                        ApplicationResponseText = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationResponseId)
                .ForeignKey("dbo.Application", t => t.ApplicationId)
                .ForeignKey("dbo.ApplicationSectionQuestionAnswer", t => t.ApplicationSectionQuestionAnswerId)
                .ForeignKey("dbo.ApplicationSectionQuestion", t => t.ApplicationSectionQuestionId)
                .Index(t => t.ApplicationId)
                .Index(t => t.ApplicationSectionQuestionId)
                .Index(t => t.ApplicationSectionQuestionAnswerId);
            
            CreateTable(
                "dbo.ApplicationStatus",
                c => new
                    {
                        ApplicationStatusId = c.Int(nullable: false, identity: true),
                        ApplicationStatusName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationStatusId);
            
            AddColumn("dbo.Application", "ApplicationStatusId", c => c.Int(nullable: false));
            CreateIndex("dbo.Application", "ApplicationStatusId");
            AddForeignKey("dbo.Application", "ApplicationStatusId", "dbo.ApplicationStatus", "ApplicationStatusId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Application", "ApplicationStatusId", "dbo.ApplicationStatus");
            DropForeignKey("dbo.ApplicationResponse", "ApplicationSectionQuestionId", "dbo.ApplicationSectionQuestion");
            DropForeignKey("dbo.ApplicationResponse", "ApplicationSectionQuestionAnswerId", "dbo.ApplicationSectionQuestionAnswer");
            DropForeignKey("dbo.ApplicationResponse", "ApplicationId", "dbo.Application");
            DropIndex("dbo.ApplicationResponse", new[] { "ApplicationSectionQuestionAnswerId" });
            DropIndex("dbo.ApplicationResponse", new[] { "ApplicationSectionQuestionId" });
            DropIndex("dbo.ApplicationResponse", new[] { "ApplicationId" });
            DropIndex("dbo.Application", new[] { "ApplicationStatusId" });
            DropColumn("dbo.Application", "ApplicationStatusId");
            DropTable("dbo.ApplicationStatus");
            DropTable("dbo.ApplicationResponse");
        }
    }
}
