namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationSection",
                c => new
                    {
                        ApplicationSectionId = c.Guid(nullable: false),
                        ApplicationTypeId = c.Int(nullable: false),
                        ParentApplicationSectionId = c.Guid(),
                        ApplicationSectionPartNumber = c.Int(nullable: false),
                        ApplicationSectionName = c.String(nullable: false, maxLength: 300),
                        ApplicationSectionIsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationSectionId)
                .ForeignKey("dbo.ApplicationType", t => t.ApplicationTypeId)
                .ForeignKey("dbo.ApplicationSection", t => t.ParentApplicationSectionId)
                .Index(t => t.ApplicationTypeId)
                .Index(t => t.ParentApplicationSectionId);
            
            CreateTable(
                "dbo.ApplicationSectionQuestion",
                c => new
                    {
                        ApplicationSectionQuestionId = c.Guid(nullable: false),
                        ApplicationSectionId = c.Guid(nullable: false),
                        QuestionTypeId = c.Int(nullable: false),
                        ApplicationSectionQuestionText = c.String(nullable: false, maxLength: 1000),
                        ApplicationSectionQuestionDescription = c.String(),
                        ApplicationSectionQuestionIsActive = c.Boolean(nullable: false),
                        ApplicationSectionQuestionOrder = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationSectionQuestionId)
                .ForeignKey("dbo.ApplicationSection", t => t.ApplicationSectionId)
                .ForeignKey("dbo.QuestionType", t => t.QuestionTypeId)
                .Index(t => t.ApplicationSectionId)
                .Index(t => t.QuestionTypeId);
            
            CreateTable(
                "dbo.ApplicationSectionQuestionAnswer",
                c => new
                    {
                        ApplicationSectionQuestionAnswerId = c.Guid(nullable: false),
                        ApplicationSectionQuestionId = c.Guid(nullable: false),
                        ApplicationSectionQuestionAnswerText = c.String(nullable: false, maxLength: 1000),
                        ApplicationSectionQuestionAnswerIsActive = c.Boolean(nullable: false),
                        ApplicationSectionQuestionAnswerOrder = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationSectionQuestionAnswerId)
                .ForeignKey("dbo.ApplicationSectionQuestion", t => t.ApplicationSectionQuestionId)
                .Index(t => t.ApplicationSectionQuestionId);
            
            CreateTable(
                "dbo.QuestionType",
                c => new
                    {
                        QuestionTypeId = c.Int(nullable: false, identity: true),
                        QuestionTypeName = c.String(nullable: false, maxLength: 100),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.QuestionTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationSectionQuestion", "QuestionTypeId", "dbo.QuestionType");
            DropForeignKey("dbo.ApplicationSectionQuestion", "ApplicationSectionId", "dbo.ApplicationSection");
            DropForeignKey("dbo.ApplicationSectionQuestionAnswer", "ApplicationSectionQuestionId", "dbo.ApplicationSectionQuestion");
            DropForeignKey("dbo.ApplicationSection", "ParentApplicationSectionId", "dbo.ApplicationSection");
            DropForeignKey("dbo.ApplicationSection", "ApplicationTypeId", "dbo.ApplicationType");
            DropIndex("dbo.ApplicationSectionQuestionAnswer", new[] { "ApplicationSectionQuestionId" });
            DropIndex("dbo.ApplicationSectionQuestion", new[] { "QuestionTypeId" });
            DropIndex("dbo.ApplicationSectionQuestion", new[] { "ApplicationSectionId" });
            DropIndex("dbo.ApplicationSection", new[] { "ParentApplicationSectionId" });
            DropIndex("dbo.ApplicationSection", new[] { "ApplicationTypeId" });
            DropTable("dbo.QuestionType");
            DropTable("dbo.ApplicationSectionQuestionAnswer");
            DropTable("dbo.ApplicationSectionQuestion");
            DropTable("dbo.ApplicationSection");
        }
    }
}
