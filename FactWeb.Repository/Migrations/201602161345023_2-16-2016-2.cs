namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _21620162 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Distance", "UserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Distance", "UserId");
            AddForeignKey("dbo.Distance", "UserId", "dbo.User", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Distance", "UserId", "dbo.User");
            DropIndex("dbo.Distance", new[] { "UserId" });
            DropColumn("dbo.Distance", "UserId");
        }
    }
}
