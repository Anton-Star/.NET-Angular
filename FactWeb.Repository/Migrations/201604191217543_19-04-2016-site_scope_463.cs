namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _19042016site_scope_463 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Site", "ScopeId", c => c.Int());
            CreateIndex("dbo.Site", "ScopeId");
            AddForeignKey("dbo.Site", "ScopeId", "dbo.ScopeType", "ScopeTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Site", "ScopeId", "dbo.ScopeType");
            DropIndex("dbo.Site", new[] { "ScopeId" });
            DropColumn("dbo.Site", "ScopeId");
        }
    }
}
