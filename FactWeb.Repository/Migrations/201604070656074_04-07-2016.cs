namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04072016 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FacilityType", newName: "MasterServiceType");
            RenameColumn(table: "dbo.MasterServiceType", name: "FacilityTypeId", newName: "MasterServiceTypeId");
            RenameColumn(table: "dbo.MasterServiceType", name: "FacilityTypeName", newName: "MasterServiceTypeName");
            RenameColumn(table: "dbo.MasterServiceType", name: "FacilityTypeShortName", newName: "MasterServiceTypeShortName");
            RenameColumn(table: "dbo.Facility", name: "FacilityTypeId", newName: "MasterServiceTypeId");
            RenameColumn(table: "dbo.ServiceType", name: "FacilityTypeId", newName: "MasterServiceTypeId");
            RenameIndex(table: "dbo.Facility", name: "IX_FacilityTypeId", newName: "IX_MasterServiceTypeId");
            RenameIndex(table: "dbo.ServiceType", name: "IX_FacilityTypeId", newName: "IX_MasterServiceTypeId");
            AlterColumn("dbo.Facility", "FacilityNumber", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Facility", "FacilityNumber", c => c.String());
            RenameIndex(table: "dbo.ServiceType", name: "IX_MasterServiceTypeId", newName: "IX_FacilityTypeId");
            RenameIndex(table: "dbo.Facility", name: "IX_MasterServiceTypeId", newName: "IX_FacilityTypeId");
            RenameColumn(table: "dbo.ServiceType", name: "MasterServiceTypeId", newName: "FacilityTypeId");
            RenameColumn(table: "dbo.Facility", name: "MasterServiceTypeId", newName: "FacilityTypeId");
            RenameColumn(table: "dbo.MasterServiceType", name: "MasterServiceTypeShortName", newName: "FacilityTypeShortName");
            RenameColumn(table: "dbo.MasterServiceType", name: "MasterServiceTypeName", newName: "FacilityTypeName");
            RenameColumn(table: "dbo.MasterServiceType", name: "MasterServiceTypeId", newName: "FacilityTypeId");
            RenameTable(name: "dbo.MasterServiceType", newName: "FacilityType");
        }
    }
}
