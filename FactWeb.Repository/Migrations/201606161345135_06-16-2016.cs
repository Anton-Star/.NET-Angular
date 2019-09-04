namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06162016 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationResponseTrainee", "ApplicationResponseId", "dbo.ApplicationResponse");
            DropIndex("dbo.ApplicationResponseTrainee", new[] { "ApplicationResponseId" });
            AddColumn("dbo.ApplicationResponseTrainee", "ApplicationId", c => c.Int(nullable: false));
            AddColumn("dbo.ApplicationResponseTrainee", "ApplicationSectionQuestionId", c => c.Guid(nullable: false));
            AddColumn("dbo.ApplicationResponseTrainee", "ApplicationSectionQuestionAnswerId", c => c.Guid());
            CreateIndex("dbo.ApplicationResponseTrainee", "ApplicationId");
            CreateIndex("dbo.ApplicationResponseTrainee", "ApplicationSectionQuestionId");
            CreateIndex("dbo.ApplicationResponseTrainee", "ApplicationSectionQuestionAnswerId");
            AddForeignKey("dbo.ApplicationResponseTrainee", "ApplicationId", "dbo.Application", "ApplicationId");
            AddForeignKey("dbo.ApplicationResponseTrainee", "ApplicationSectionQuestionId", "dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionId");
            AddForeignKey("dbo.ApplicationResponseTrainee", "ApplicationSectionQuestionAnswerId", "dbo.ApplicationSectionQuestionAnswer", "ApplicationSectionQuestionAnswerId");
            DropColumn("dbo.ApplicationResponseTrainee", "ApplicationResponseId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationResponseTrainee", "ApplicationResponseId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ApplicationResponseTrainee", "ApplicationSectionQuestionAnswerId", "dbo.ApplicationSectionQuestionAnswer");
            DropForeignKey("dbo.ApplicationResponseTrainee", "ApplicationSectionQuestionId", "dbo.ApplicationSectionQuestion");
            DropForeignKey("dbo.ApplicationResponseTrainee", "ApplicationId", "dbo.Application");
            DropIndex("dbo.ApplicationResponseTrainee", new[] { "ApplicationSectionQuestionAnswerId" });
            DropIndex("dbo.ApplicationResponseTrainee", new[] { "ApplicationSectionQuestionId" });
            DropIndex("dbo.ApplicationResponseTrainee", new[] { "ApplicationId" });
            DropColumn("dbo.ApplicationResponseTrainee", "ApplicationSectionQuestionAnswerId");
            DropColumn("dbo.ApplicationResponseTrainee", "ApplicationSectionQuestionId");
            DropColumn("dbo.ApplicationResponseTrainee", "ApplicationId");
            CreateIndex("dbo.ApplicationResponseTrainee", "ApplicationResponseId");
            AddForeignKey("dbo.ApplicationResponseTrainee", "ApplicationResponseId", "dbo.ApplicationResponse", "ApplicationResponseId");
        }
    }
}
