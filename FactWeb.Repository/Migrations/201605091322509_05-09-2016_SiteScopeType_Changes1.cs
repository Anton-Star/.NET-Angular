namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05092016_SiteScopeType_Changes1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SiteScopeType", "ScopeTypeId", "dbo.ScopeType");
            DropForeignKey("dbo.SiteScopeType", "SiteId", "dbo.Site");
            DropIndex("dbo.SiteScopeType", new[] { "SiteId" });
            DropIndex("dbo.SiteScopeType", new[] { "ScopeTypeId" });
            DropTable("dbo.SiteScopeType");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SiteScopeType",
                c => new
                    {
                        SiteScopeTypeId = c.Int(nullable: false, identity: true),
                        SiteId = c.Int(nullable: false),
                        ScopeTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiteScopeTypeId);
            
            CreateIndex("dbo.SiteScopeType", "ScopeTypeId");
            CreateIndex("dbo.SiteScopeType", "SiteId");
            AddForeignKey("dbo.SiteScopeType", "SiteId", "dbo.Site", "SiteId");
            AddForeignKey("dbo.SiteScopeType", "ScopeTypeId", "dbo.ScopeType", "ScopeTypeId");
        }
    }
}
