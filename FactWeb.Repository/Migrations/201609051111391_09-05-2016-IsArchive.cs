namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _09052016IsArchive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionSchedule", "InspectionScheduleIsArchive", c => c.Boolean(nullable: false));
            AddColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailIsArchive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailIsArchive");
            DropColumn("dbo.InspectionSchedule", "InspectionScheduleIsArchive");
        }
    }
}
