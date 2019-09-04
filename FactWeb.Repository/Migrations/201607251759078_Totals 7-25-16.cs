namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Totals72516 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteCollectionTotal",
                c => new
                    {
                        SiteCollectionTotalId = c.Guid(nullable: false),
                        SiteId = c.Int(nullable: false),
                        CollectionTypeId = c.Guid(nullable: false),
                        ClinicalPopulationTypeId = c.Int(nullable: false),
                        SiteCollectionTotalNumberOfUnits = c.Int(nullable: false),
                        SiteCollectionTotalAsOfDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiteCollectionTotalId)
                .ForeignKey("dbo.ClinicalPopulationType", t => t.ClinicalPopulationTypeId)
                .ForeignKey("dbo.CollectionType", t => t.CollectionTypeId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.CollectionTypeId)
                .Index(t => t.ClinicalPopulationTypeId);
            
            CreateTable(
                "dbo.CollectionType",
                c => new
                    {
                        CollectionTypeId = c.Guid(nullable: false),
                        CollectionTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CollectionTypeId);
            
            CreateTable(
                "dbo.SiteProcessingMethodologyTotal",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProcessingTypeId = c.Int(nullable: false),
                        PlatformCount = c.Int(nullable: false),
                        ProtocolCount = c.Int(nullable: false),
                        AsOfDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProcessingType", t => t.ProcessingTypeId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.ProcessingTypeId);
            
            CreateTable(
                "dbo.SiteProcessingTotal",
                c => new
                    {
                        SiteProcessingTotalId = c.Guid(nullable: false),
                        SiteId = c.Int(nullable: false),
                        CellTypeId = c.Guid(nullable: false),
                        SiteProcessingTotalNumberOfUnits = c.Int(nullable: false),
                        SiteProcessingTotalAsOfDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                        TransplantCellType_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.SiteProcessingTotalId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.TransplantCellType", t => t.TransplantCellType_Id)
                .Index(t => t.SiteId)
                .Index(t => t.TransplantCellType_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiteProcessingTotal", "TransplantCellType_Id", "dbo.TransplantCellType");
            DropForeignKey("dbo.SiteProcessingTotal", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteProcessingMethodologyTotal", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteProcessingMethodologyTotal", "ProcessingTypeId", "dbo.ProcessingType");
            DropForeignKey("dbo.SiteCollectionTotal", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteCollectionTotal", "CollectionTypeId", "dbo.CollectionType");
            DropForeignKey("dbo.SiteCollectionTotal", "ClinicalPopulationTypeId", "dbo.ClinicalPopulationType");
            DropIndex("dbo.SiteProcessingTotal", new[] { "TransplantCellType_Id" });
            DropIndex("dbo.SiteProcessingTotal", new[] { "SiteId" });
            DropIndex("dbo.SiteProcessingMethodologyTotal", new[] { "ProcessingTypeId" });
            DropIndex("dbo.SiteProcessingMethodologyTotal", new[] { "SiteId" });
            DropIndex("dbo.SiteCollectionTotal", new[] { "ClinicalPopulationTypeId" });
            DropIndex("dbo.SiteCollectionTotal", new[] { "CollectionTypeId" });
            DropIndex("dbo.SiteCollectionTotal", new[] { "SiteId" });
            DropTable("dbo.SiteProcessingTotal");
            DropTable("dbo.SiteProcessingMethodologyTotal");
            DropTable("dbo.CollectionType");
            DropTable("dbo.SiteCollectionTotal");
        }
    }
}
