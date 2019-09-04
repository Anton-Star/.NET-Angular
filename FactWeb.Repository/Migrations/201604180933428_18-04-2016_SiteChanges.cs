namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18042016_SiteChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Site", new[] { "ClinicalTypeId" });
            DropIndex("dbo.Site", new[] { "ProcessingTypeId" });
            DropIndex("dbo.Site", new[] { "CollectionProductTypeId" });
            DropIndex("dbo.Site", new[] { "CBCollectionTypeId" });
            DropIndex("dbo.Site", new[] { "ClinicalPopulationTypeId" });
            DropIndex("dbo.Site", new[] { "TransplantTypeId" });
            CreateTable(
                "dbo.CBUnitType",
                c => new
                    {
                        CBUnitTypeId = c.Int(nullable: false, identity: true),
                        CBUnitTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CBUnitTypeId);
            
            AddColumn("dbo.Site", "SiteProvince", c => c.String());
            AddColumn("dbo.Site", "CBUnitTypeId", c => c.Int());
            AddColumn("dbo.Site", "CBUnitsBanked", c => c.Int());
            AddColumn("dbo.Site", "CBUnitsBankDate", c => c.DateTime());
            AlterColumn("dbo.Site", "ClinicalTypeId", c => c.Int());
            AlterColumn("dbo.Site", "ProcessingTypeId", c => c.Int());
            AlterColumn("dbo.Site", "CollectionProductTypeId", c => c.Int());
            AlterColumn("dbo.Site", "CBCollectionTypeId", c => c.Int());
            AlterColumn("dbo.Site", "ClinicalPopulationTypeId", c => c.Int());
            AlterColumn("dbo.Site", "TransplantTypeId", c => c.Int());
            CreateIndex("dbo.Site", "ClinicalTypeId");
            CreateIndex("dbo.Site", "ProcessingTypeId");
            CreateIndex("dbo.Site", "CollectionProductTypeId");
            CreateIndex("dbo.Site", "ClinicalPopulationTypeId");
            CreateIndex("dbo.Site", "TransplantTypeId");
            CreateIndex("dbo.Site", "CBCollectionTypeId");
            CreateIndex("dbo.Site", "CBUnitTypeId");
            AddForeignKey("dbo.Site", "CBUnitTypeId", "dbo.CBUnitType", "CBUnitTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Site", "CBUnitTypeId", "dbo.CBUnitType");
            DropIndex("dbo.Site", new[] { "CBUnitTypeId" });
            DropIndex("dbo.Site", new[] { "CBCollectionTypeId" });
            DropIndex("dbo.Site", new[] { "TransplantTypeId" });
            DropIndex("dbo.Site", new[] { "ClinicalPopulationTypeId" });
            DropIndex("dbo.Site", new[] { "CollectionProductTypeId" });
            DropIndex("dbo.Site", new[] { "ProcessingTypeId" });
            DropIndex("dbo.Site", new[] { "ClinicalTypeId" });
            AlterColumn("dbo.Site", "TransplantTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Site", "ClinicalPopulationTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Site", "CBCollectionTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Site", "CollectionProductTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Site", "ProcessingTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Site", "ClinicalTypeId", c => c.Int(nullable: false));
            DropColumn("dbo.Site", "CBUnitsBankDate");
            DropColumn("dbo.Site", "CBUnitsBanked");
            DropColumn("dbo.Site", "CBUnitTypeId");
            DropColumn("dbo.Site", "SiteProvince");
            DropTable("dbo.CBUnitType");
            CreateIndex("dbo.Site", "TransplantTypeId");
            CreateIndex("dbo.Site", "ClinicalPopulationTypeId");
            CreateIndex("dbo.Site", "CBCollectionTypeId");
            CreateIndex("dbo.Site", "CollectionProductTypeId");
            CreateIndex("dbo.Site", "ProcessingTypeId");
            CreateIndex("dbo.Site", "ClinicalTypeId");
        }
    }
}
