namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01022016 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.InspectionScheduleDetail", new[] { "User_Id" });
            DropColumn("dbo.InspectionScheduleDetail", "UserId");
            RenameColumn(table: "dbo.InspectionScheduleDetail", name: "User_Id", newName: "UserId");
            AddColumn("dbo.InspectionSchedule", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.InspectionScheduleDetail", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.InspectionScheduleDetail", "UserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.InspectionScheduleDetail", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.InspectionScheduleDetail", new[] { "UserId" });
            AlterColumn("dbo.InspectionScheduleDetail", "UserId", c => c.Guid());
            AlterColumn("dbo.InspectionScheduleDetail", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.InspectionSchedule", "IsActive");
            RenameColumn(table: "dbo.InspectionScheduleDetail", name: "UserId", newName: "User_Id");
            AddColumn("dbo.InspectionScheduleDetail", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.InspectionScheduleDetail", "User_Id");
        }
    }
}
