namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05092016_SiteScopeType_Changes2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteScopeType",
                c => new
                    {
                        SiteScopeTypeId = c.Guid(nullable: false),
                        SiteId = c.Int(nullable: false),
                        ScopeTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiteScopeTypeId)
                .ForeignKey("dbo.ScopeType", t => t.ScopeTypeId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.ScopeTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiteScopeType", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteScopeType", "ScopeTypeId", "dbo.ScopeType");
            DropIndex("dbo.SiteScopeType", new[] { "ScopeTypeId" });
            DropIndex("dbo.SiteScopeType", new[] { "SiteId" });
            DropTable("dbo.SiteScopeType");
        }
    }
}
