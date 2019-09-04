namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationResponseUser32816 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationResponse", "UserId", c => c.Guid());
            CreateIndex("dbo.ApplicationResponse", "UserId");
            AddForeignKey("dbo.ApplicationResponse", "UserId", "dbo.User", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationResponse", "UserId", "dbo.User");
            DropIndex("dbo.ApplicationResponse", new[] { "UserId" });
            DropColumn("dbo.ApplicationResponse", "UserId");
        }
    }
}
