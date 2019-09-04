namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacilityCibmtr : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FacilityCibmtr",
                c => new
                    {
                        FacilityCibmtrId = c.Guid(nullable: false),
                        FacilityId = c.Int(nullable: false),
                        FacilityCibmtrCenterNumber = c.String(nullable: false),
                        FacilityCibmtrCcnName = c.String(),
                        FacilityCibmtrTransplantSurvivalReportName = c.String(),
                        FacilityCibmtrDisplayName = c.String(),
                        FacilityCibmtrIsNonCibmtr = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FacilityCibmtrId)
                .ForeignKey("dbo.Facility", t => t.FacilityId)
                .Index(t => t.FacilityId);
            
            CreateTable(
                "dbo.FacilityCibmtrDataManagement",
                c => new
                    {
                        FacilityCibmtrDataManagementId = c.Guid(nullable: false),
                        FacilityCibmtrId = c.Guid(nullable: false),
                        FacilityCibmtrDataManagementAuditDate = c.DateTime(),
                        FacilityCibmtrDataManagementCriticalFieldErrorRate = c.Decimal(precision: 18, scale: 2),
                        FacilityCibmtrDataManagementRandomFieldErrorRate = c.Decimal(precision: 18, scale: 2),
                        FacilityCibmtrDataManagementOverallFieldErrorRate = c.Decimal(precision: 18, scale: 2),
                        FacilityCibmtrDataManagementIsCapIdentified = c.Boolean(),
                        FacilityCibmtrDataManagementAuditorComments = c.String(),
                        FacilityCibmtrDataManagementCpiLetterDate = c.DateTime(),
                        CpiTypeId = c.Guid(),
                        FacilityCibmtrDataManagementCpiComments = c.String(),
                        FacilityCibmtrDataManagementCorrectiveActions = c.String(),
                        FacilityCibmtrDataManagementFactProgressDetermination = c.String(),
                        FacilityCibmtrDataManagementIsAuditAccuracyRequired = c.Boolean(),
                        FacilityCibmtrDataManagementAdditionalInformation = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FacilityCibmtrDataManagementId)
                .ForeignKey("dbo.CpiType", t => t.CpiTypeId)
                .ForeignKey("dbo.FacilityCibmtr", t => t.FacilityCibmtrId)
                .Index(t => t.FacilityCibmtrId)
                .Index(t => t.CpiTypeId);
            
            CreateTable(
                "dbo.CpiType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FacilityCibmtrOutcomeAnalysis",
                c => new
                    {
                        FacilityOutcomeAnalysisId = c.Guid(nullable: false),
                        FacilityCibmtrId = c.Guid(nullable: false),
                        FacilityOutcomeAnalysisReportYear = c.Int(),
                        FacilityOutcomeAnalysisSurvivalScore = c.Int(),
                        FacilityOutcomeAnalysisSampleSize = c.Int(),
                        FacilityOutcomeAnalysisActualPercent = c.Decimal(precision: 18, scale: 2),
                        FacilityOutcomeAnalysisPredictedPercent = c.Decimal(precision: 18, scale: 2),
                        FacilityOutcomeAnalysisLowerPercent = c.Decimal(precision: 18, scale: 2),
                        FacilityOutcomeAnalysisUpperPercent = c.Decimal(precision: 18, scale: 2),
                        FacilityOutcomeAnalysisComparativeDataSource = c.String(),
                        FacilityOutcomeAnalysisPublishedOneYearSurvival = c.String(),
                        FacilityOutcomeAnalysisProgramOneYearSurvival = c.String(),
                        FacilityOutcomeAnalysisComments = c.String(),
                        FacilityOutcomeAnalysisReportedCausesOfDeath = c.String(),
                        FacilityOutcomeAnalysisCorrectiveActions = c.String(),
                        FacilityOutcomeAnalysisFactImprovementPlan = c.String(),
                        FacilityOutcomeAnalysisAdditionalInformationRequested = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FacilityOutcomeAnalysisId)
                .ForeignKey("dbo.FacilityCibmtr", t => t.FacilityCibmtrId)
                .Index(t => t.FacilityCibmtrId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FacilityCibmtrOutcomeAnalysis", "FacilityCibmtrId", "dbo.FacilityCibmtr");
            DropForeignKey("dbo.FacilityCibmtrDataManagement", "FacilityCibmtrId", "dbo.FacilityCibmtr");
            DropForeignKey("dbo.FacilityCibmtrDataManagement", "CpiTypeId", "dbo.CpiType");
            DropForeignKey("dbo.FacilityCibmtr", "FacilityId", "dbo.Facility");
            DropIndex("dbo.FacilityCibmtrOutcomeAnalysis", new[] { "FacilityCibmtrId" });
            DropIndex("dbo.FacilityCibmtrDataManagement", new[] { "CpiTypeId" });
            DropIndex("dbo.FacilityCibmtrDataManagement", new[] { "FacilityCibmtrId" });
            DropIndex("dbo.FacilityCibmtr", new[] { "FacilityId" });
            DropTable("dbo.FacilityCibmtrOutcomeAnalysis");
            DropTable("dbo.CpiType");
            DropTable("dbo.FacilityCibmtrDataManagement");
            DropTable("dbo.FacilityCibmtr");
        }
    }
}
