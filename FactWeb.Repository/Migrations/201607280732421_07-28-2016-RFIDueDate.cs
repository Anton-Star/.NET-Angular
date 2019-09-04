namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _07282016RFIDueDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "ApplicationRFIDueDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Application", "ApplicationRFIDueDate");
        }
    }
}
