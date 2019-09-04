namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComplianceApplicationReportReviewStaus : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ComplianceApplication", "AccreditationStatusId", "dbo.AccreditationStatus");
            DropIndex("dbo.ComplianceApplication", new[] { "AccreditationStatusId" });
            AddColumn("dbo.ComplianceApplication", "ReportReviewStatusId", c => c.Int());
            CreateIndex("dbo.ComplianceApplication", "ReportReviewStatusId");
            AddForeignKey("dbo.ComplianceApplication", "ReportReviewStatusId", "dbo.ReportReviewStatus", "ReportReviewStatusId");
            DropColumn("dbo.ComplianceApplication", "AccreditationStatusId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ComplianceApplication", "AccreditationStatusId", c => c.Int());
            DropForeignKey("dbo.ComplianceApplication", "ReportReviewStatusId", "dbo.ReportReviewStatus");
            DropIndex("dbo.ComplianceApplication", new[] { "ReportReviewStatusId" });
            DropColumn("dbo.ComplianceApplication", "ReportReviewStatusId");
            CreateIndex("dbo.ComplianceApplication", "AccreditationStatusId");
            AddForeignKey("dbo.ComplianceApplication", "AccreditationStatusId", "dbo.AccreditationStatus", "AccreditationStatusId");
        }
    }
}
