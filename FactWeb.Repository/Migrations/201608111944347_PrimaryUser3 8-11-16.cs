namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimaryUser381116 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Organization", new[] { "PrimaryUserId" });
            AlterColumn("dbo.Organization", "PrimaryUserId", c => c.Guid());
            CreateIndex("dbo.Organization", "PrimaryUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Organization", new[] { "PrimaryUserId" });
            AlterColumn("dbo.Organization", "PrimaryUserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Organization", "PrimaryUserId");
        }
    }
}
