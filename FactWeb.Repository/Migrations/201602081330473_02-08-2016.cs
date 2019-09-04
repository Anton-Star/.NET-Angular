namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02082016 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.OrganizationFacility", name: "StrongRelation", newName: "OrganizationFacilityStrongRelation");
            RenameColumn(table: "dbo.InspectionScheduleDetail", name: "IsLead", newName: "InspectionScheduleDetailIsLead");
            RenameColumn(table: "dbo.InspectionScheduleDetail", name: "IsMentor", newName: "InspectionScheduleDetailIsMentor");
            RenameColumn(table: "dbo.InspectionSchedule", name: "InspectionDate", newName: "InspectionScheduleInspectionDate");
            RenameColumn(table: "dbo.InspectionSchedule", name: "IsCompleted", newName: "InspectionScheduleIsCompleted");
            RenameColumn(table: "dbo.InspectionSchedule", name: "CompletionDate", newName: "InspectionScheduleCompletionDate");
            RenameColumn(table: "dbo.InspectionSchedule", name: "IsActive", newName: "InspectionScheduleIsActive");
            AddColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailIsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InspectionScheduleDetail", "InspectionScheduleDetailIsActive");
            RenameColumn(table: "dbo.InspectionSchedule", name: "InspectionScheduleIsActive", newName: "IsActive");
            RenameColumn(table: "dbo.InspectionSchedule", name: "InspectionScheduleCompletionDate", newName: "CompletionDate");
            RenameColumn(table: "dbo.InspectionSchedule", name: "InspectionScheduleIsCompleted", newName: "IsCompleted");
            RenameColumn(table: "dbo.InspectionSchedule", name: "InspectionScheduleInspectionDate", newName: "InspectionDate");
            RenameColumn(table: "dbo.InspectionScheduleDetail", name: "InspectionScheduleDetailIsMentor", newName: "IsMentor");
            RenameColumn(table: "dbo.InspectionScheduleDetail", name: "InspectionScheduleDetailIsLead", newName: "IsLead");
            RenameColumn(table: "dbo.OrganizationFacility", name: "OrganizationFacilityStrongRelation", newName: "StrongRelation");
        }
    }
}
