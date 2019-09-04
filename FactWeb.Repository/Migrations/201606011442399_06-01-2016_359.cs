namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06012016_359 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComplianceApplication", "ComplianceApplicationAccreditationStatus", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ComplianceApplication", "ComplianceApplicationAccreditationStatus");
        }
    }
}
