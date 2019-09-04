namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CibmtrChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrDataManagementCriticalFieldErrorRate", c => c.Single());
            AlterColumn("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrDataManagementRandomFieldErrorRate", c => c.Single());
            AlterColumn("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrDataManagementOverallFieldErrorRate", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrDataManagementOverallFieldErrorRate", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrDataManagementRandomFieldErrorRate", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrDataManagementCriticalFieldErrorRate", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
