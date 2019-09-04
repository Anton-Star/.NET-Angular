namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationChanges3232016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionComplianceNumber", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSectionQuestion", "ApplicationSectionQuestionComplianceNumber");
        }
    }
}
