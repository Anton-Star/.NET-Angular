namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InspectionSchedule101916 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailMentorFeedback", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailMentorFeedback");
        }
    }
}
