namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _050520161 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationResponseComment", "ApplicationResponseId", "dbo.ApplicationResponse");
            DropIndex("dbo.ApplicationResponseComment", new[] { "ApplicationResponseId" });
            AddColumn("dbo.ApplicationResponseComment", "ApplicationId", c => c.Int());
            AddColumn("dbo.ApplicationResponseComment", "QuestionId", c => c.Guid());
            CreateIndex("dbo.ApplicationResponseComment", "ApplicationId");
            CreateIndex("dbo.ApplicationResponseComment", "QuestionId");
            AddForeignKey("dbo.ApplicationResponseComment", "ApplicationId", "dbo.Application", "ApplicationId");
            AddForeignKey("dbo.ApplicationResponseComment", "QuestionId", "dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionId");
            DropColumn("dbo.ApplicationResponseComment", "ApplicationResponseId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationResponseComment", "ApplicationResponseId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ApplicationResponseComment", "QuestionId", "dbo.ApplicationSectionQuestion");
            DropForeignKey("dbo.ApplicationResponseComment", "ApplicationId", "dbo.Application");
            DropIndex("dbo.ApplicationResponseComment", new[] { "QuestionId" });
            DropIndex("dbo.ApplicationResponseComment", new[] { "ApplicationId" });
            DropColumn("dbo.ApplicationResponseComment", "QuestionId");
            DropColumn("dbo.ApplicationResponseComment", "ApplicationId");
            CreateIndex("dbo.ApplicationResponseComment", "ApplicationResponseId");
            AddForeignKey("dbo.ApplicationResponseComment", "ApplicationResponseId", "dbo.ApplicationResponse", "ApplicationResponseId");
        }
    }
}
