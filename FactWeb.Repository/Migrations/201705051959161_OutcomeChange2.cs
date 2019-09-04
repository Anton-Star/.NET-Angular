namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OutcomeChange2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrDataManagementProgressOnImplementation", c => c.String());
            AddColumn("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrDataManagementInspectorInformation", c => c.String());
            AddColumn("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrDataManagementInspectorCommendablePractices", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrDataManagementInspectorCommendablePractices");
            DropColumn("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrDataManagementInspectorInformation");
            DropColumn("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrDataManagementProgressOnImplementation");
        }
    }
}
