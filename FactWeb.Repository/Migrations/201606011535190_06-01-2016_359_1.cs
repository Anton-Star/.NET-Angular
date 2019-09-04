namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06012016_359_1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ComplianceApplication", "ComplianceApplicationAccreditationStatus", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ComplianceApplication", "ComplianceApplicationAccreditationStatus", c => c.Int());
        }
    }
}
