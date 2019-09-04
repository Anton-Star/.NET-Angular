namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InspectionTrainee82416 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inspection", "InspectionIsTrainee", c => c.Boolean());
            AddColumn("dbo.Inspection", "InspectionTraineeSiteDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inspection", "InspectionTraineeSiteDescription");
            DropColumn("dbo.Inspection", "InspectionIsTrainee");
        }
    }
}
