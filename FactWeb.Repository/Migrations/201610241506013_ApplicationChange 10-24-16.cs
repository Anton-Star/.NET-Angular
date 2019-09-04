namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationChange102416 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "ApplicationIsActive", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Application", "ApplicationIsActive");
        }
    }
}
