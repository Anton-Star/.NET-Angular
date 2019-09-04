namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03142016_Site_Changes_367_1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CBCollectionType",
                c => new
                    {
                        CBCollectionTypeId = c.Int(nullable: false, identity: true),
                        CBCollectionTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CBCollectionTypeId);
            
            CreateTable(
                "dbo.ClinicalPopulationType",
                c => new
                    {
                        ClinicalPopulationTypeId = c.Int(nullable: false, identity: true),
                        ClinicalPopulationTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ClinicalPopulationTypeId);
            
            CreateTable(
                "dbo.ClinicalType",
                c => new
                    {
                        ClinicalTypeId = c.Int(nullable: false, identity: true),
                        ClinicalTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ClinicalTypeId);
            
            CreateTable(
                "dbo.CollectionProductType",
                c => new
                    {
                        CollectionProductTypeId = c.Int(nullable: false, identity: true),
                        CollectionProductTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CollectionProductTypeId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.FacilitySite",
                c => new
                    {
                        FacilitySiteId = c.Int(nullable: false, identity: true),
                        FacilityId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FacilitySiteId)
                .ForeignKey("dbo.Facility", t => t.FacilityId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.FacilityId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.Site",
                c => new
                    {
                        SiteId = c.Int(nullable: false, identity: true),
                        SiteName = c.String(),
                        SiteStartDate = c.DateTime(nullable: false),
                        SitePhone = c.String(),
                        SiteStreetAddress = c.String(),
                        SiteCity = c.String(),
                        StateId = c.Int(),
                        SiteZip = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        SiteIsPrimarySite = c.Boolean(nullable: false),
                        ClinicalTypeId = c.Int(nullable: false),
                        ProcessingTypeId = c.Int(nullable: false),
                        CollectionProductTypeId = c.Int(nullable: false),
                        CBCollectionTypeId = c.Int(nullable: false),
                        ClinicalPopulationTypeId = c.Int(nullable: false),
                        TransplantTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiteId)
                .ForeignKey("dbo.CBCollectionType", t => t.CBCollectionTypeId)
                .ForeignKey("dbo.ClinicalPopulationType", t => t.ClinicalPopulationTypeId)
                .ForeignKey("dbo.ClinicalType", t => t.ClinicalTypeId)
                .ForeignKey("dbo.CollectionProductType", t => t.CollectionProductTypeId)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .ForeignKey("dbo.ProcessingType", t => t.ProcessingTypeId)
                .ForeignKey("dbo.State", t => t.StateId)
                .ForeignKey("dbo.TransplantType", t => t.TransplantTypeId)
                .Index(t => t.StateId)
                .Index(t => t.CountryId)
                .Index(t => t.ClinicalTypeId)
                .Index(t => t.ProcessingTypeId)
                .Index(t => t.CollectionProductTypeId)
                .Index(t => t.CBCollectionTypeId)
                .Index(t => t.ClinicalPopulationTypeId)
                .Index(t => t.TransplantTypeId);
            
            CreateTable(
                "dbo.ProcessingType",
                c => new
                    {
                        ProcessingTypeId = c.Int(nullable: false, identity: true),
                        ProcessingTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProcessingTypeId);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        StateName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StateId);
            
            CreateTable(
                "dbo.TransplantType",
                c => new
                    {
                        TransplantTypeId = c.Int(nullable: false, identity: true),
                        TransplantTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransplantTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FacilitySite", "SiteId", "dbo.Site");
            DropForeignKey("dbo.Site", "TransplantTypeId", "dbo.TransplantType");
            DropForeignKey("dbo.Site", "StateId", "dbo.State");
            DropForeignKey("dbo.Site", "ProcessingTypeId", "dbo.ProcessingType");
            DropForeignKey("dbo.Site", "CountryId", "dbo.Country");
            DropForeignKey("dbo.Site", "CollectionProductTypeId", "dbo.CollectionProductType");
            DropForeignKey("dbo.Site", "ClinicalTypeId", "dbo.ClinicalType");
            DropForeignKey("dbo.Site", "ClinicalPopulationTypeId", "dbo.ClinicalPopulationType");
            DropForeignKey("dbo.Site", "CBCollectionTypeId", "dbo.CBCollectionType");
            DropForeignKey("dbo.FacilitySite", "FacilityId", "dbo.Facility");
            DropIndex("dbo.Site", new[] { "TransplantTypeId" });
            DropIndex("dbo.Site", new[] { "ClinicalPopulationTypeId" });
            DropIndex("dbo.Site", new[] { "CBCollectionTypeId" });
            DropIndex("dbo.Site", new[] { "CollectionProductTypeId" });
            DropIndex("dbo.Site", new[] { "ProcessingTypeId" });
            DropIndex("dbo.Site", new[] { "ClinicalTypeId" });
            DropIndex("dbo.Site", new[] { "CountryId" });
            DropIndex("dbo.Site", new[] { "StateId" });
            DropIndex("dbo.FacilitySite", new[] { "SiteId" });
            DropIndex("dbo.FacilitySite", new[] { "FacilityId" });
            DropTable("dbo.TransplantType");
            DropTable("dbo.State");
            DropTable("dbo.ProcessingType");
            DropTable("dbo.Site");
            DropTable("dbo.FacilitySite");
            DropTable("dbo.Country");
            DropTable("dbo.CollectionProductType");
            DropTable("dbo.ClinicalType");
            DropTable("dbo.ClinicalPopulationType");
            DropTable("dbo.CBCollectionType");
        }
    }
}
