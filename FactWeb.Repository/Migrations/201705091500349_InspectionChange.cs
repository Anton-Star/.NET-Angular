namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InspectionChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailReviewedOutcomesData", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailReviewedOutcomesData");
        }
    }
}
