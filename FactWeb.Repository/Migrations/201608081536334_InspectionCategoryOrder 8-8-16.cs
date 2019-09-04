namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InspectionCategoryOrder8816 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionCategory", "InspectionCategoryReportingOrder", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InspectionCategory", "InspectionCategoryReportingOrder");
        }
    }
}
