namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OutcomeChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FacilityCibmtrOutcomeAnalysis", "FacilityOutcomeAnalysisProgressOnImplementation", c => c.String());
            AddColumn("dbo.FacilityCibmtrOutcomeAnalysis", "FacilityOutcomeAnalysisInspectorInformation", c => c.String());
            AddColumn("dbo.FacilityCibmtrOutcomeAnalysis", "FacilityOutcomeAnalysisInspectorCommendablePractices", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FacilityCibmtrOutcomeAnalysis", "FacilityOutcomeAnalysisInspectorCommendablePractices");
            DropColumn("dbo.FacilityCibmtrOutcomeAnalysis", "FacilityOutcomeAnalysisInspectorInformation");
            DropColumn("dbo.FacilityCibmtrOutcomeAnalysis", "FacilityOutcomeAnalysisProgressOnImplementation");
        }
    }
}
