namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrgChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organization", "OrganizationUseTwoYearCycle", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organization", "OrganizationUseTwoYearCycle");
        }
    }
}
