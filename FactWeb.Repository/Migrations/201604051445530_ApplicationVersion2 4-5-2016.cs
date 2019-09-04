namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationVersion2452016 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ApplicationSection", new[] { "ApplicationVersionId" });
            AlterColumn("dbo.ApplicationSection", "ApplicationVersionId", c => c.Guid(nullable: false));
            CreateIndex("dbo.ApplicationSection", "ApplicationVersionId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ApplicationSection", new[] { "ApplicationVersionId" });
            AlterColumn("dbo.ApplicationSection", "ApplicationVersionId", c => c.Guid());
            CreateIndex("dbo.ApplicationSection", "ApplicationVersionId");
        }
    }
}
