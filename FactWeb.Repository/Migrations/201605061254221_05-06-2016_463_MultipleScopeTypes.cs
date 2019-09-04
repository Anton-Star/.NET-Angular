namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05062016_463_MultipleScopeTypes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Site", "ScopeId", "dbo.ScopeType");
            DropIndex("dbo.Site", new[] { "ScopeId" });
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
                .PrimaryKey(t => t.SiteScopeTypeId)
                .ForeignKey("dbo.ScopeType", t => t.ScopeTypeId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.ScopeTypeId);
            
            DropColumn("dbo.Site", "ScopeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Site", "ScopeId", c => c.Int());
            DropForeignKey("dbo.SiteScopeType", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteScopeType", "ScopeTypeId", "dbo.ScopeType");
            DropIndex("dbo.SiteScopeType", new[] { "ScopeTypeId" });
            DropIndex("dbo.SiteScopeType", new[] { "SiteId" });
            DropTable("dbo.SiteScopeType");
            CreateIndex("dbo.Site", "ScopeId");
            AddForeignKey("dbo.Site", "ScopeId", "dbo.ScopeType", "ScopeTypeId");
        }
    }
}
