namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimaryUser281116 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organization", "PrimaryUserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Organization", "PrimaryUserId");
            AddForeignKey("dbo.Organization", "PrimaryUserId", "dbo.User", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organization", "PrimaryUserId", "dbo.User");
            DropIndex("dbo.Organization", new[] { "PrimaryUserId" });
            DropColumn("dbo.Organization", "PrimaryUserId");
        }
    }
}
