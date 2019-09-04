namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02092016 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Facility", name: "Name", newName: "FacilityName");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Facility", name: "FacilityName", newName: "Name");
        }
    }
}
