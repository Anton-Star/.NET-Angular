namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastReminderDate11116 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailLastReminderDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailLastReminderDate");
        }
    }
}
