namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationQuestionFlags42016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSectionQuestion", "QuestionTypesFlag", c => c.Byte());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSectionQuestion", "QuestionTypesFlag");
        }
    }
}
