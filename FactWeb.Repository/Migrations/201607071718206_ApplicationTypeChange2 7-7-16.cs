namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationTypeChange27716 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationType", "IsManageable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationType", "IsManageable", c => c.Boolean());
        }
    }
}
