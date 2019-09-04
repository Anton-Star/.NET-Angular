namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SiteChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Site", "ProcessingTypeId", "dbo.ProcessingType");
            DropIndex("dbo.Site", new[] { "ProcessingTypeId" });
            CreateTable(
                "dbo.SiteProcessingType",
                c => new
                    {
                        SiteProcessingTypeId = c.Guid(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ProcessingTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiteProcessingTypeId)
                .ForeignKey("dbo.ProcessingType", t => t.ProcessingTypeId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.ProcessingTypeId);
            
            DropColumn("dbo.Site", "ProcessingTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Site", "ProcessingTypeId", c => c.Int());
            DropForeignKey("dbo.SiteProcessingType", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteProcessingType", "ProcessingTypeId", "dbo.ProcessingType");
            DropIndex("dbo.SiteProcessingType", new[] { "ProcessingTypeId" });
            DropIndex("dbo.SiteProcessingType", new[] { "SiteId" });
            DropTable("dbo.SiteProcessingType");
            CreateIndex("dbo.Site", "ProcessingTypeId");
            AddForeignKey("dbo.Site", "ProcessingTypeId", "dbo.ProcessingType", "ProcessingTypeId");
        }
    }
}
