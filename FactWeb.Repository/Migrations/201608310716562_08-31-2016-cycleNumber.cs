namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _08312016cycleNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "ApplicationCycleNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Application", "ApplicationCycleNumber");
        }
    }
}
