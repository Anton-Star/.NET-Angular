namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SiteTotals72116 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteCordBloodTransplantTotal",
                c => new
                    {
                        SiteCordBloodTransplantTotalId = c.Guid(nullable: false),
                        SiteId = c.Int(nullable: false),
                        CBUnitTypeId = c.Int(nullable: false),
                        CBCategoryId = c.Int(nullable: false),
                        SiteCordBloodTransplantTotalNumberOfUnits = c.Int(nullable: false),
                        SiteCordBloodTransplantTotalAsOfDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiteCordBloodTransplantTotalId)
                .ForeignKey("dbo.CBCategory", t => t.CBCategoryId)
                .ForeignKey("dbo.CBUnitType", t => t.CBUnitTypeId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.CBUnitTypeId)
                .Index(t => t.CBCategoryId);
            
            CreateTable(
                "dbo.CBCategory",
                c => new
                    {
                        CBCategoryId = c.Int(nullable: false, identity: true),
                        CBCategoryName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CBCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiteCordBloodTransplantTotal", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteCordBloodTransplantTotal", "CBUnitTypeId", "dbo.CBUnitType");
            DropForeignKey("dbo.SiteCordBloodTransplantTotal", "CBCategoryId", "dbo.CBCategory");
            DropIndex("dbo.SiteCordBloodTransplantTotal", new[] { "CBCategoryId" });
            DropIndex("dbo.SiteCordBloodTransplantTotal", new[] { "CBUnitTypeId" });
            DropIndex("dbo.SiteCordBloodTransplantTotal", new[] { "SiteId" });
            DropTable("dbo.CBCategory");
            DropTable("dbo.SiteCordBloodTransplantTotal");
        }
    }
}
