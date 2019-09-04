namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimaryFacility83116 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facility", "PrimaryOrganizationId", c => c.Int());
            CreateIndex("dbo.Facility", "PrimaryOrganizationId");
            AddForeignKey("dbo.Facility", "PrimaryOrganizationId", "dbo.Organization", "OrganizationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Facility", "PrimaryOrganizationId", "dbo.Organization");
            DropIndex("dbo.Facility", new[] { "PrimaryOrganizationId" });
            DropColumn("dbo.Facility", "PrimaryOrganizationId");
        }
    }
}
