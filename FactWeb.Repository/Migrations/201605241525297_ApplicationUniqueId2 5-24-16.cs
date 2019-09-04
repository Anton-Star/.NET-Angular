namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUniqueId252416 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Application", "ApplicationUniqueId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Application", "ApplicationUniqueId", c => c.Guid());
        }
    }
}
