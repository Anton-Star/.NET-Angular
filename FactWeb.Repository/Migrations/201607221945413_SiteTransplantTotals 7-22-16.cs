namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SiteTransplantTotals72216 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteTransplantTotal",
                c => new
                    {
                        SiteTransplantTotalId = c.Guid(nullable: false),
                        SiteId = c.Int(nullable: false),
                        TransplantCellTypeId = c.Guid(nullable: false),
                        ClinicalPopulationTypeId = c.Int(nullable: false),
                        TransplantTypeId = c.Int(nullable: false),
                        SiteTransplantTotalIsHaploid = c.Boolean(nullable: false),
                        SiteTransplantTotalNumberOfUnits = c.Int(nullable: false),
                        SiteTransplantTotalAsOfDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiteTransplantTotalId)
                .ForeignKey("dbo.ClinicalPopulationType", t => t.ClinicalPopulationTypeId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.TransplantCellType", t => t.TransplantCellTypeId)
                .ForeignKey("dbo.TransplantType", t => t.TransplantTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.TransplantCellTypeId)
                .Index(t => t.ClinicalPopulationTypeId)
                .Index(t => t.TransplantTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiteTransplantTotal", "TransplantTypeId", "dbo.TransplantType");
            DropForeignKey("dbo.SiteTransplantTotal", "TransplantCellTypeId", "dbo.TransplantCellType");
            DropForeignKey("dbo.SiteTransplantTotal", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteTransplantTotal", "ClinicalPopulationTypeId", "dbo.ClinicalPopulationType");
            DropIndex("dbo.SiteTransplantTotal", new[] { "TransplantTypeId" });
            DropIndex("dbo.SiteTransplantTotal", new[] { "ClinicalPopulationTypeId" });
            DropIndex("dbo.SiteTransplantTotal", new[] { "TransplantCellTypeId" });
            DropIndex("dbo.SiteTransplantTotal", new[] { "SiteId" });
            DropTable("dbo.SiteTransplantTotal");
        }
    }
}
