namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InspectionScheduleDetailUpdate71916 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailIsInspectionComplete", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailIsInspectionComplete");
        }
    }
}
