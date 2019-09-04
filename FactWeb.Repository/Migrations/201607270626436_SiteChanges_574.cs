namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SiteChanges_574 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Site", "ClinicalTypeId", "dbo.ClinicalType");
            DropForeignKey("dbo.Site", "TransplantTypeId", "dbo.TransplantType");
            DropIndex("dbo.Site", new[] { "ClinicalTypeId" });
            DropIndex("dbo.Site", new[] { "TransplantTypeId" });
            CreateTable(
                "dbo.SiteClinicalType",
                c => new
                    {
                        SiteClinicalTypeId = c.Guid(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ClinicalTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiteClinicalTypeId)
                .ForeignKey("dbo.ClinicalType", t => t.ClinicalTypeId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.ClinicalTypeId);
            
            CreateTable(
                "dbo.SiteTransplantType",
                c => new
                    {
                        SiteTransplantTypeId = c.Guid(nullable: false),
                        SiteId = c.Int(nullable: false),
                        TransplantTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiteTransplantTypeId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.TransplantType", t => t.TransplantTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.TransplantTypeId);
            
            DropColumn("dbo.Site", "ClinicalTypeId");
            DropColumn("dbo.Site", "TransplantTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Site", "TransplantTypeId", c => c.Int());
            AddColumn("dbo.Site", "ClinicalTypeId", c => c.Int());
            DropForeignKey("dbo.SiteTransplantType", "TransplantTypeId", "dbo.TransplantType");
            DropForeignKey("dbo.SiteTransplantType", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteClinicalType", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteClinicalType", "ClinicalTypeId", "dbo.ClinicalType");
            DropIndex("dbo.SiteTransplantType", new[] { "TransplantTypeId" });
            DropIndex("dbo.SiteTransplantType", new[] { "SiteId" });
            DropIndex("dbo.SiteClinicalType", new[] { "ClinicalTypeId" });
            DropIndex("dbo.SiteClinicalType", new[] { "SiteId" });
            DropTable("dbo.SiteTransplantType");
            DropTable("dbo.SiteClinicalType");
            CreateIndex("dbo.Site", "TransplantTypeId");
            CreateIndex("dbo.Site", "ClinicalTypeId");
            AddForeignKey("dbo.Site", "TransplantTypeId", "dbo.TransplantType", "TransplantTypeId");
            AddForeignKey("dbo.Site", "ClinicalTypeId", "dbo.ClinicalType", "ClinicalTypeId");
        }
    }
}
