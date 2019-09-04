namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InspectionScheduleDetailUpdate271916 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailInspectorCompletionDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailInspectorCompletionDate");
        }
    }
}
