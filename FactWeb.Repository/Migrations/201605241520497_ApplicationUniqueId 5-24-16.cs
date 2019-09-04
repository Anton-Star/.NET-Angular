namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUniqueId52416 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "ApplicationUniqueId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Application", "ApplicationUniqueId");
        }
    }
}
