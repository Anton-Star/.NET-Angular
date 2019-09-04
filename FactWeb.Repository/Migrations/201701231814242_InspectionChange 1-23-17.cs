namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InspectionChange12317 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inspection", "InspectionIsReinspection", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inspection", "InspectionIsReinspection");
        }
    }
}
