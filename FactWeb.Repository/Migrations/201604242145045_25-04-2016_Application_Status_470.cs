namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _25042016_Application_Status_470 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationStatus", "ApplicationStatusNameForApplicant", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationStatus", "ApplicationStatusNameForApplicant");
        }
    }
}
