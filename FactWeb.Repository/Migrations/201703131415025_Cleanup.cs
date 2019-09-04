namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Cleanup : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.ApplicationResponseComment", "QuestionId", c => c.Guid());
            //AddColumn("dbo.ApplicationResponseComment", "ApplicationId", c => c.Int());
            //CreateIndex("dbo.ApplicationResponseComment", "QuestionId");
            //CreateIndex("dbo.ApplicationResponseComment", "ApplicationId");
            //AddForeignKey("dbo.ApplicationResponseComment", "QuestionId", "dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionId");
            //AddForeignKey("dbo.ApplicationResponseComment", "ApplicationId", "dbo.Application", "ApplicationId");
            //DropForeignKey("dbo.ApplicationResponseComment", "ApplicationResponseId", "dbo.ApplicationResponse");
            //DropIndex("dbo.ApplicationResponseComment", new[] { "ApplicationResponseId" });
            //DropColumn("dbo.ApplicationResponseComment", "ApplicationResponseId");
        }
        
        public override void Down()
        {
        }
    }
}
