namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Totals272516 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.SiteProcessingTotal", new[] { "TransplantCellType_Id" });
            RenameColumn(table: "dbo.SiteProcessingTotal", name: "TransplantCellType_Id", newName: "TransplantCellTypeId");
            AlterColumn("dbo.SiteProcessingTotal", "TransplantCellTypeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.SiteProcessingTotal", "TransplantCellTypeId");
            DropColumn("dbo.SiteProcessingTotal", "CellTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SiteProcessingTotal", "CellTypeId", c => c.Guid(nullable: false));
            DropIndex("dbo.SiteProcessingTotal", new[] { "TransplantCellTypeId" });
            AlterColumn("dbo.SiteProcessingTotal", "TransplantCellTypeId", c => c.Guid());
            RenameColumn(table: "dbo.SiteProcessingTotal", name: "TransplantCellTypeId", newName: "TransplantCellType_Id");
            CreateIndex("dbo.SiteProcessingTotal", "TransplantCellType_Id");
        }
    }
}
