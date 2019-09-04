namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrgAccreditedSince : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organization", "OrganizationAccreditedSinceDate", c => c.DateTime());            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organization", "OrganizationAccreditedSinceDate");            
        }
    }
}
