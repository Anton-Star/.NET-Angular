namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationAnswerChanges3232016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSectionQuestionAnswer", "ApplicationSectionQuestionAnswerIsExpectedAnswer", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSectionQuestionAnswer", "ApplicationSectionQuestionAnswerIsExpectedAnswer");
        }
    }
}
