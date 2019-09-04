namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _28_1_2016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionSchedule", "Organizations_Id", c => c.Int());
            CreateIndex("dbo.InspectionSchedule", "ApplicationId");
            CreateIndex("dbo.InspectionSchedule", "Organizations_Id");
            AddForeignKey("dbo.InspectionSchedule", "ApplicationId", "dbo.Application", "ApplicationId");
            AddForeignKey("dbo.InspectionSchedule", "Organizations_Id", "dbo.Organization", "OrganizationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InspectionSchedule", "Organizations_Id", "dbo.Organization");
            DropForeignKey("dbo.InspectionSchedule", "ApplicationId", "dbo.Application");
            DropIndex("dbo.InspectionSchedule", new[] { "Organizations_Id" });
            DropIndex("dbo.InspectionSchedule", new[] { "ApplicationId" });
            DropColumn("dbo.InspectionSchedule", "Organizations_Id");
        }
    }
}
