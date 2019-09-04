namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Application51716 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "ApplicationSubmittedDate", c => c.DateTime());
            AddColumn("dbo.Application", "ApplicationDueDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Application", "ApplicationDueDate");
            DropColumn("dbo.Application", "ApplicationSubmittedDate");
        }
    }
}
