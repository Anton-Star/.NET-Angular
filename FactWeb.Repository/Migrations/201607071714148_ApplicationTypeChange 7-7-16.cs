namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationTypeChange7716 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationType", "IsManageable", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationType", "IsManageable");
        }
    }
}
