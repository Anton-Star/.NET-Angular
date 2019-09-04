namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06172016_540 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "ApplicationVersionId", c => c.Guid());
            CreateIndex("dbo.Application", "ApplicationVersionId");
            AddForeignKey("dbo.Application", "ApplicationVersionId", "dbo.ApplicationVersion", "ApplicationVersionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Application", "ApplicationVersionId", "dbo.ApplicationVersion");
            DropIndex("dbo.Application", new[] { "ApplicationVersionId" });
            DropColumn("dbo.Application", "ApplicationVersionId");
        }
    }
}
