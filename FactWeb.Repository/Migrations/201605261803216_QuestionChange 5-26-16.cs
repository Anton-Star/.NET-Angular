namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionChange52616 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionText", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionText", c => c.String(nullable: false, maxLength: 1000));
        }
    }
}
