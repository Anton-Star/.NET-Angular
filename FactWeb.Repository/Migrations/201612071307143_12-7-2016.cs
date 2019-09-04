namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1272016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComplianceApplication", "AccreditationStatusId", c => c.Int());
            CreateIndex("dbo.ComplianceApplication", "AccreditationStatusId");
            AddForeignKey("dbo.ComplianceApplication", "AccreditationStatusId", "dbo.AccreditationStatus", "AccreditationStatusId");
            DropColumn("dbo.ComplianceApplication", "ComplianceApplicationAccreditationStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ComplianceApplication", "ComplianceApplicationAccreditationStatus", c => c.String());
            DropForeignKey("dbo.ComplianceApplication", "AccreditationStatusId", "dbo.AccreditationStatus");
            DropIndex("dbo.ComplianceApplication", new[] { "AccreditationStatusId" });
            DropColumn("dbo.ComplianceApplication", "AccreditationStatusId");
        }
    }
}
