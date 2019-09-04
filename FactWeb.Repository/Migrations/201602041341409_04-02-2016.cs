namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04022016 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.InspectionScheduleDetail", new[] { "AccreditationRole_Id" });
            DropIndex("dbo.InspectionSchedule", new[] { "Organizations_Id" });
            RenameColumn(table: "dbo.InspectionScheduleDetail", name: "AccreditationRole_Id", newName: "AccreditationRoleId");
            RenameColumn(table: "dbo.InspectionSchedule", name: "Organizations_Id", newName: "OrganizationId");
            AlterColumn("dbo.InspectionScheduleDetail", "AccreditationRoleId", c => c.Int(nullable: false));
            AlterColumn("dbo.InspectionSchedule", "OrganizationId", c => c.Int(nullable: false));
            CreateIndex("dbo.InspectionScheduleDetail", "AccreditationRoleId");
            CreateIndex("dbo.InspectionSchedule", "OrganizationId");
            DropColumn("dbo.InspectionScheduleDetail", "AccreditatioRoleId");
            DropColumn("dbo.InspectionSchedule", "OrganziationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InspectionSchedule", "OrganziationId", c => c.Int(nullable: false));
            AddColumn("dbo.InspectionScheduleDetail", "AccreditatioRoleId", c => c.Int(nullable: false));
            DropIndex("dbo.InspectionSchedule", new[] { "OrganizationId" });
            DropIndex("dbo.InspectionScheduleDetail", new[] { "AccreditationRoleId" });
            AlterColumn("dbo.InspectionSchedule", "OrganizationId", c => c.Int());
            AlterColumn("dbo.InspectionScheduleDetail", "AccreditationRoleId", c => c.Int());
            RenameColumn(table: "dbo.InspectionSchedule", name: "OrganizationId", newName: "Organizations_Id");
            RenameColumn(table: "dbo.InspectionScheduleDetail", name: "AccreditationRoleId", newName: "AccreditationRole_Id");
            CreateIndex("dbo.InspectionSchedule", "Organizations_Id");
            CreateIndex("dbo.InspectionScheduleDetail", "AccreditationRole_Id");
        }
    }
}
