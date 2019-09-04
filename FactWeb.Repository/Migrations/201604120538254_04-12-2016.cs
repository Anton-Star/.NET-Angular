namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04122016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScopeType", "ScopeTypeIsArchived", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScopeType", "ScopeTypeIsArchived");
        }
    }
}
