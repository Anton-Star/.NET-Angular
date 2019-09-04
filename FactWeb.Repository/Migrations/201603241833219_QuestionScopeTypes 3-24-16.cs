namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionScopeTypes32416 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationSectionQuestionScopeType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationSectionQuestionId = c.Guid(nullable: false),
                        ScopeTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationSectionQuestion", t => t.ApplicationSectionQuestionId)
                .ForeignKey("dbo.ScopeType", t => t.ScopeTypeId)
                .Index(t => t.ApplicationSectionQuestionId)
                .Index(t => t.ScopeTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationSectionQuestionScopeType", "ScopeTypeId", "dbo.ScopeType");
            DropForeignKey("dbo.ApplicationSectionQuestionScopeType", "ApplicationSectionQuestionId", "dbo.ApplicationSectionQuestion");
            DropIndex("dbo.ApplicationSectionQuestionScopeType", new[] { "ScopeTypeId" });
            DropIndex("dbo.ApplicationSectionQuestionScopeType", new[] { "ApplicationSectionQuestionId" });
            DropTable("dbo.ApplicationSectionQuestionScopeType");
        }
    }
}
