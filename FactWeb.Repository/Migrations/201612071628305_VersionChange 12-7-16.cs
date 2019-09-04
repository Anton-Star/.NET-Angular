namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VersionChange12716 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationVersion", "ApplicationVersionIsDeleted", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationVersion", "ApplicationVersionIsDeleted");
        }
    }
}
