namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Outcomes52516 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccreditationOutcome",
                c => new
                    {
                        AccreditationOutcomeId = c.Int(nullable: false, identity: true),
                        OrganizationId = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        OutcomeStatusId = c.Int(nullable: false),
                        ReportReviewStatusId = c.Int(nullable: false),
                        AccreditationOutcomeCommitteeDate = c.DateTime(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AccreditationOutcomeId)
                .ForeignKey("dbo.Application", t => t.ApplicationId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .ForeignKey("dbo.OutcomeStatus", t => t.OutcomeStatusId)
                .ForeignKey("dbo.ReportReviewStatus", t => t.ReportReviewStatusId)
                .Index(t => t.OrganizationId)
                .Index(t => t.ApplicationId)
                .Index(t => t.OutcomeStatusId)
                .Index(t => t.ReportReviewStatusId);
            
            CreateTable(
                "dbo.OutcomeStatus",
                c => new
                    {
                        OutcomeStatusId = c.Int(nullable: false, identity: true),
                        OutcomeStatusName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OutcomeStatusId);
            
            CreateTable(
                "dbo.ReportReviewStatus",
                c => new
                    {
                        ReportReviewStatusId = c.Int(nullable: false, identity: true),
                        ReportReviewStatusName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ReportReviewStatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccreditationOutcome", "ReportReviewStatusId", "dbo.ReportReviewStatus");
            DropForeignKey("dbo.AccreditationOutcome", "OutcomeStatusId", "dbo.OutcomeStatus");
            DropForeignKey("dbo.AccreditationOutcome", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.AccreditationOutcome", "ApplicationId", "dbo.Application");
            DropIndex("dbo.AccreditationOutcome", new[] { "ReportReviewStatusId" });
            DropIndex("dbo.AccreditationOutcome", new[] { "OutcomeStatusId" });
            DropIndex("dbo.AccreditationOutcome", new[] { "ApplicationId" });
            DropIndex("dbo.AccreditationOutcome", new[] { "OrganizationId" });
            DropTable("dbo.ReportReviewStatus");
            DropTable("dbo.OutcomeStatus");
            DropTable("dbo.AccreditationOutcome");
        }
    }
}
