namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03132016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionSchedule", "InspectionScheduleStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.InspectionSchedule", "InspectionScheduleEndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailInspectionDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailInspectionDate");
            DropColumn("dbo.InspectionSchedule", "InspectionScheduleEndDate");
            DropColumn("dbo.InspectionSchedule", "InspectionScheduleStartDate");
        }
    }
}
