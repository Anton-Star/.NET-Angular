namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobFunctionReporting10516 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobFunction", "JobFunctionIncludeInReporting", c => c.Boolean());
            AddColumn("dbo.JobFunction", "JobFunctionReportingOrder", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobFunction", "JobFunctionReportingOrder");
            DropColumn("dbo.JobFunction", "JobFunctionIncludeInReporting");
        }
    }
}
