namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationChange61016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "CoordinatorId", c => c.Guid());
            CreateIndex("dbo.Application", "CoordinatorId");
            AddForeignKey("dbo.Application", "CoordinatorId", "dbo.User", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Application", "CoordinatorId", "dbo.User");
            DropIndex("dbo.Application", new[] { "CoordinatorId" });
            DropColumn("dbo.Application", "CoordinatorId");
        }
    }
}
