namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TotalChange8216 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.SiteProcessingTotal", new[] { "TransplantCellTypeId" });
            RenameColumn(table: "dbo.SiteProcessingMethodologyTotal", name: "Id", newName: "SiteProcessingMethodologyTotalId");
            RenameColumn(table: "dbo.SiteProcessingMethodologyTotal", name: "PlatformCount", newName: "SiteProcessingMethodologyTotalPlatformCount");
            RenameColumn(table: "dbo.SiteProcessingMethodologyTotal", name: "ProtocolCount", newName: "SiteProcessingMethodologyTotalProtocolCount");
            CreateTable(
                "dbo.SiteProcessingTotalTransplantCellType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SiteProcessingTotalId = c.Guid(nullable: false),
                        TransplantCellTypeId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SiteProcessingTotal", t => t.SiteProcessingTotalId)
                .ForeignKey("dbo.TransplantCellType", t => t.TransplantCellTypeId)
                .Index(t => t.SiteProcessingTotalId)
                .Index(t => t.TransplantCellTypeId);
            
            AddColumn("dbo.SiteCollectionTotal", "SiteCollectionTotalStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SiteCollectionTotal", "SiteCollectionTotalEndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SiteProcessingMethodologyTotal", "SiteProcessingMethodologyTotalStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SiteProcessingMethodologyTotal", "SiteProcessingMethodologyTotalEndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SiteProcessingTotal", "SiteProcessingTotalStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SiteProcessingTotal", "SiteProcessingTotalEndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SiteTransplantTotal", "SiteTransplantTotalStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SiteTransplantTotal", "SiteTransplantTotalEndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SiteProcessingTotal", "TransplantCellTypeId", c => c.Guid());
            CreateIndex("dbo.SiteProcessingTotal", "TransplantCellTypeId");
            DropColumn("dbo.SiteCollectionTotal", "SiteCollectionTotalAsOfDate");
            DropColumn("dbo.SiteProcessingMethodologyTotal", "AsOfDate");
            DropColumn("dbo.SiteProcessingTotal", "SiteProcessingTotalAsOfDate");
            DropColumn("dbo.SiteTransplantTotal", "SiteTransplantTotalAsOfDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SiteTransplantTotal", "SiteTransplantTotalAsOfDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SiteProcessingTotal", "SiteProcessingTotalAsOfDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SiteProcessingMethodologyTotal", "AsOfDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SiteCollectionTotal", "SiteCollectionTotalAsOfDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.SiteProcessingTotalTransplantCellType", "TransplantCellTypeId", "dbo.TransplantCellType");
            DropForeignKey("dbo.SiteProcessingTotalTransplantCellType", "SiteProcessingTotalId", "dbo.SiteProcessingTotal");
            DropIndex("dbo.SiteProcessingTotalTransplantCellType", new[] { "TransplantCellTypeId" });
            DropIndex("dbo.SiteProcessingTotalTransplantCellType", new[] { "SiteProcessingTotalId" });
            DropIndex("dbo.SiteProcessingTotal", new[] { "TransplantCellTypeId" });
            AlterColumn("dbo.SiteProcessingTotal", "TransplantCellTypeId", c => c.Guid(nullable: false));
            DropColumn("dbo.SiteTransplantTotal", "SiteTransplantTotalEndDate");
            DropColumn("dbo.SiteTransplantTotal", "SiteTransplantTotalStartDate");
            DropColumn("dbo.SiteProcessingTotal", "SiteProcessingTotalEndDate");
            DropColumn("dbo.SiteProcessingTotal", "SiteProcessingTotalStartDate");
            DropColumn("dbo.SiteProcessingMethodologyTotal", "SiteProcessingMethodologyTotalEndDate");
            DropColumn("dbo.SiteProcessingMethodologyTotal", "SiteProcessingMethodologyTotalStartDate");
            DropColumn("dbo.SiteCollectionTotal", "SiteCollectionTotalEndDate");
            DropColumn("dbo.SiteCollectionTotal", "SiteCollectionTotalStartDate");
            DropTable("dbo.SiteProcessingTotalTransplantCellType");
            RenameColumn(table: "dbo.SiteProcessingMethodologyTotal", name: "SiteProcessingMethodologyTotalProtocolCount", newName: "ProtocolCount");
            RenameColumn(table: "dbo.SiteProcessingMethodologyTotal", name: "SiteProcessingMethodologyTotalPlatformCount", newName: "PlatformCount");
            RenameColumn(table: "dbo.SiteProcessingMethodologyTotal", name: "SiteProcessingMethodologyTotalId", newName: "Id");
            CreateIndex("dbo.SiteProcessingTotal", "TransplantCellTypeId");
        }
    }
}
